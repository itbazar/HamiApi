using Domain.Models.IdentityAggregate;

namespace Application.Common.Interfaces.Security;

public interface IAuthenticationService
{
    public Task<LoginResultModel> Login(string username, string password, bool twoFactorEnabled = false);
    public Task<VerificationToken> LogisterCitizen(string phoneNumber);
    public Task<AuthToken> VerifyOtp(string otpToken, string code);
    public Task<AuthToken> Refresh(string token, string refreshToken);
    public Task<bool> Revoke(string userId, string refreshToken);
    public Task<bool> ChangePassword(string username, string oldPassword, string newPassword);
}

public record LoginResultModel(
    AuthToken? AuthToken,
    VerificationToken? VerificationToken);
public record AuthToken(string JwtToken, string RefreshToken);
public record VerificationToken(string PhoneNumber, string Token, string Code);