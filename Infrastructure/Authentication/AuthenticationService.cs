using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtInfo _jwtInfo;
    
    public AuthenticationService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, JwtInfo jwtInfo)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _jwtInfo = jwtInfo;
    }

    public async Task<LoginResultModel> Login(string username, string password, string? verificationCode = null)
    {
        var user = await GetUser(username);
        if (!user.PhoneNumberConfirmed)
        {
            if(verificationCode is null)
            {
                throw new PhoneNumberNotConfirmedException("Confirm the phone number first.");
            }
            else
            {
                if(await ValidateOtp(user, verificationCode))
                {
                    user.PhoneNumberConfirmed = true;
                    await _unitOfWork.SaveAsync();
                }
            }
            
        }
        if (await _userManager.CheckPasswordAsync(user, password))
        {
            return await GenerateToken(user);
        }
        else
        {
            throw new InvalidLoginException();
        }
    }

    public async Task<bool> RegisterCitizen(string username, string password)
    {
        Regex regex = new Regex(@"^09[0-9]{9}$");
        if (!regex.IsMatch(username))
        {
            throw new InvalidUsernameException();
        }
        var userExists = await _userManager.FindByNameAsync(username);
        if (userExists is not null && userExists.PhoneNumberConfirmed)
            throw new UserAlreadyExsistsException();

        if (userExists is not null)
        {
            await _userManager.RemovePasswordAsync(userExists);
            await _userManager.AddPasswordAsync(userExists, password);
            return true;
        }

        ApplicationUser user = new ApplicationUser()
        {
            //Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = username,
            PhoneNumber = username,
            PhoneNumberConfirmed = false
        };
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            new UserRegisterException();

        var result2 = await _userManager.AddToRoleAsync(user, "Citizen");

        if (!result2.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            new UserRegisterException();
        }

        return true;
    }

    public async Task<string> GetVerificationCode(string username)
    {
        var user = await GetUser(username);
        return await GenerateOtp(user);
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

        var token = new JwtSecurityToken(
            issuer: _jwtInfo.Issuer,
            audience: _jwtInfo.Audience,
            expires: DateTime.Now.Add(_jwtInfo.TimeSpan),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

        return new LoginResultModel(new JwtSecurityTokenHandler().WriteToken(token));
    }
}

public record JwtInfo(string Secret, string Issuer, string Audience, TimeSpan TimeSpan);

