using Application.Common.Exceptions;
using Application.Common.Interfaces.Security;
using Application.Common.Statics;
using Domain.Models.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtInfo _jwtInfo;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly IAuthenticateRepository _authenticateRepository;

    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        JwtInfo jwtInfo,
        TokenValidationParameters tokenValidationParameters,
        IAuthenticateRepository authenticateRepository)
    {
        _userManager = userManager;
        _jwtInfo = jwtInfo;
        _tokenValidationParameters = tokenValidationParameters;
        _authenticateRepository = authenticateRepository;
    }

    public async Task<LoginResultModel> Login(string username, string password, bool twoFactorEnabled = false)
    {
        var user = await GetUser(username);

        if (await _userManager.CheckPasswordAsync(user, password))
        {
            if (twoFactorEnabled)
            {
                return new LoginResultModel(null, await GetVerificationCode(user));
            }
            else
            {
                return new LoginResultModel(await GenerateToken(user), null);
            }
        }
        else
        {
            throw new InvalidLoginException();
        }
    }

    public async Task<AuthToken> VerifyOtp(string otpToken, string code)
    {
        var storedOtp = await _authenticateRepository.GetOtpAsync(otpToken);
        if (storedOtp is null)
            throw new Exception("Otp not found");

        if (await ValidateOtp(storedOtp.User, code))
        {
            await _authenticateRepository.DeleteOtpAsync(otpToken);
            if (storedOtp.IsNew)
            {
                await CreateCitizen(storedOtp.User);
            }
            return await GenerateToken(storedOtp.User);
        }
            
        throw new InvalidVerificationCode();
    }

    public async Task<VerificationToken> LogisterCitizen(string phoneNumber)
    {
        Regex regex = new Regex(@"^09[0-9]{9}$");
        if (!regex.IsMatch(phoneNumber))
        {
            throw new InvalidUsernameException();
        }
        var user = await _userManager.FindByNameAsync(phoneNumber);
        var isNew = user is null;
        if (user is null)
            user = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = phoneNumber,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = false
            };
        return await GetVerificationCode(user, isNew);
    }

    public async Task<AuthToken> Refresh(string token, string refreshToken)
    {
        var validatedToken = GetPrincipalFromToken(token);
        if (validatedToken == null)
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

        var storedRefreshToken = await _authenticateRepository.GetRefreshTokenAsync(refreshToken);
        if (storedRefreshToken is null)
            throw new Exception("Refresh token not exsists");

        // We don't need this when using Redis but is kept for compatibility
        if (now > storedRefreshToken.CreationDate.Add(storedRefreshToken.Expiry))
            throw new Exception("Refresh token is expired");

        var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        if (storedRefreshToken.JwtId != jti)
            throw new Exception("Jwt id not matched");

        await _authenticateRepository.DeleteRefreshTokenAsync(refreshToken);

        var userName = validatedToken.FindFirstValue(ClaimTypes.Name);
        if (userName is null)
            throw new Exception("Invalid username");
        var user = await GetUser(userName);
        return await GenerateToken(user);
    }

    public async Task<bool> Revoke(string userId, string refreshToken)
    {
        var storedRefreshToken = await _authenticateRepository.GetRefreshTokenAsync(refreshToken);
        if (storedRefreshToken is null)
            throw new Exception("Refresh token not exsists");

        if (storedRefreshToken.UserId != userId)
            throw new Exception("User not matched");
        await _authenticateRepository.DeleteRefreshTokenAsync(refreshToken);

        return true;
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

    /////////////////////////
    /// Private methods
    private async Task CreateCitizen(ApplicationUser user)
    {
        try
        {
            user.PhoneNumberConfirmed = true;
            var result = await _userManager.CreateAsync(user, "ADGJL';khfs12");
            if (!result.Succeeded)
                throw new UserRegisterException();

            var result2 = await _userManager.AddToRoleAsync(user, RoleNames.Citizen);

            if (!result2.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                throw new UserRegisterException();
            }
        }
        catch
        {
            throw new UserRegisterException();
        }
    }

    private async Task<VerificationToken> GetVerificationCode(ApplicationUser user, bool isNew = false)
    {
        if (await _authenticateRepository.IsSentAsync(user.UserName!))
            throw new Exception("Verification can't be sent in less than 2 minutes.");

        var otp = new Otp(Guid.NewGuid().ToString(), user, isNew);
        await _authenticateRepository.InsertOtpAsync(otp);
        await _authenticateRepository.InsertSentAsync(user.UserName!);
        return new VerificationToken(user.PhoneNumber ?? "", otp.Token, await GenerateOtp(user));
    }

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

    private async Task<AuthToken> GenerateToken(ApplicationUser user)
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
        await _authenticateRepository.InsertRefreshTokenAsync(refreshToken);

        return new AuthToken(new JwtSecurityTokenHandler().WriteToken(token), refreshToken.Token);
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

 public record Otp(
     string Token,
     ApplicationUser User,
     bool IsNew);