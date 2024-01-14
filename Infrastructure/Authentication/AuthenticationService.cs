using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Domain.Models.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtInfo _jwtInfo;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthenticationService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, JwtInfo jwtInfo, TokenValidationParameters tokenValidationParameters, IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _jwtInfo = jwtInfo;
        _tokenValidationParameters = tokenValidationParameters;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<LoginResultModel> Login(string username, string password, string? verificationCode = null)
    {
        var user = await GetUser(username);

        if (await _userManager.CheckPasswordAsync(user, password))
        {
            if (verificationCode is null)
                throw new PhoneNumberNotConfirmedException();

            if (await ValidateOtp(user, verificationCode))
            {
                return await GenerateToken(user);
            }
            else 
            {
                throw new InvalidVerificationCode();
            }
        }
        else
        {
            throw new InvalidLoginException();
        }
    }

    public async Task<LoginResultModel> Refresh(string token, string refreshToken)
    {
        var validatedToken = GetPrincipalFromToken(token);
        if(validatedToken == null)
        {
            throw new Exception("Invalid token");
        }
        
        var now = DateTime.UtcNow;
        var expiryDateUnix =
            long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expiryDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddSeconds(expiryDateUnix);
        if (expiryDateUtc > now)
        {
            throw new Exception("Token not expired yet");
        }

        var storedRefreshToken = await _refreshTokenRepository.GetAsync(refreshToken);
        if (storedRefreshToken is null)
            throw new Exception("Refresh token not exsists");

        // We don't need this when using Redis but is kept for compatibility
        if (now > storedRefreshToken.CreationDate.Add(storedRefreshToken.Expiry))
            throw new Exception("Refresh token is expired");

        var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        if (storedRefreshToken.JwtId != jti)
            throw new Exception("Jwt id not matched");

        var userName = validatedToken.FindFirstValue(ClaimTypes.Name);
        if (userName is null)
            throw new Exception("Invalid username");
        var user = await GetUser(userName);
        return await GenerateToken(user);
    }

    public async Task<LoginResultModel> LogisterCitizen(string phoneNumber, string? verificationCode)
    {
        Regex regex = new Regex(@"^09[0-9]{9}$");
        if (!regex.IsMatch(phoneNumber))
        {
            throw new InvalidUsernameException();
        }
        var user = await _userManager.FindByNameAsync(phoneNumber);

        if (user is not null)
        {
            if (verificationCode is null)
                throw new PhoneNumberNotConfirmedException();

            if (await ValidateOtp(user, verificationCode))
            {
                return await GenerateToken(user);
            }
            else
            {
                throw new InvalidVerificationCode();
            }
        }

        user = new ApplicationUser()
        {
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = phoneNumber,
            PhoneNumber = phoneNumber,
            PhoneNumberConfirmed = false
        };
        var result = await _userManager.CreateAsync(user, "ADGJL';khfs12");
        if (!result.Succeeded)
            throw new UserRegisterException();

        var result2 = await _userManager.AddToRoleAsync(user, "Citizen");

        if (!result2.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            throw new UserRegisterException();
        }

        throw new PhoneNumberNotConfirmedException();
    }

    public async Task<VerificationCodeModel> GetVerificationCode(string username)
    {
        var user = await GetUser(username);
        var now = DateTime.UtcNow;
        if (now - user.VerificationSent < new TimeSpan(0, 2, 0))
            throw new Exception("Verification can't be sent in less than 2 minutes.");
        await _unitOfWork.DbContext.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE [dbo].[AspNetUsers] SET VerificationSent={now} WHERE Id = {user.Id}");
        return new VerificationCodeModel(user.PhoneNumber ?? "", await GenerateOtp(user));
    }

    public async Task<bool> VerifyPhoneNumber(string username, string verificationCode)
    {
        var user = await GetUser(username);
        var isVerified = await ValidateOtp(user, verificationCode);
        if (isVerified)
        {
            user.PhoneNumberConfirmed = true;
            await _unitOfWork.SaveAsync();
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> ChangePassword(string username, string oldPassword, string newPassword)
    {
        var user = await GetUser(username);
        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        if (result.Succeeded)
        {
            return true;
        }
        return false;
    }

    public async Task<string> GetResetPasswordToken(string username, string verificationCode)
    {
        var user = await GetUser(username);
        var isValid = await ValidateOtp(user, verificationCode);

        if (isValid)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if(token is null)
            {
                throw new ResetPasswordTokenGenerationFailed();
            }
            return token;
        }
        else
        {
            throw new InvalidVerificationCode();
        }
    }

    public async Task<bool> ResetPassword(string username, string resetPasswordToken, string newPassword)
    {
        var user = await GetUser(username);
        
        var result = await _userManager.ResetPasswordAsync(user, resetPasswordToken, newPassword);
        if (result.Succeeded)
            return true;
        else
            return false;
    }

    /////////////////////////
    /// Private methods
    private async Task<ApplicationUser> GetUser(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            throw new UserNotExistException();
        }

        return user;
    }

    private async Task<string> GenerateOtp(ApplicationUser user)
    {
        //TODO: Use purpose for verification, reset password, etc
        PhoneNumberTokenProvider<ApplicationUser> tokenGenerator = new PhoneNumberTokenProvider<ApplicationUser>();
        return await tokenGenerator.GenerateAsync("Phone number verification", _userManager, user);
    }

    private async Task<bool> ValidateOtp(ApplicationUser user, string token)
    {
        PhoneNumberTokenProvider<ApplicationUser> tokenGenerator = new PhoneNumberTokenProvider<ApplicationUser>();
        return await tokenGenerator.ValidateAsync("Phone number verification", token, _userManager, user);
    }

    private async Task<LoginResultModel> GenerateToken(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName ?? ""),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtInfo.Secret));
        var now = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            issuer: _jwtInfo.Issuer,
            audience: _jwtInfo.Audience,
            expires: now.Add(_jwtInfo.AccessTokenValidDuration),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

        var refreshToken = new RefreshToken(
            Guid.NewGuid().ToString(),
            user.Id,
            token.Id,
            now,
            _jwtInfo.RefreshTokenValidDuration);
        await _refreshTokenRepository.InsertAsync(refreshToken);

        return new LoginResultModel(new JwtSecurityTokenHandler().WriteToken(token), refreshToken.Token);
    }

    private ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(
                token,
                _tokenValidationParameters,
                out var validatedToken);
            if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
            {
                return null;
            }
            return principal;
        }
        catch
        {
            return null;
        }
    }
    private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
            jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);
    }
}

public record JwtInfo(
    string Secret,
    string Issuer,
    string Audience,
    TimeSpan AccessTokenValidDuration,
    TimeSpan RefreshTokenValidDuration);

public record RefreshToken(
    string Token,
    string UserId,
    string JwtId,
    DateTime CreationDate,
    TimeSpan Expiry);
