using Domain.Models.IdentityAggregate;

namespace Application.Common.Interfaces.Security;

public interface IAuthenticationService
{
    public Task<Result<LoginResultModel>> Login(string username, string password, bool twoFactorEnabled = false);
    public Task<Result<VerificationToken>> LogisterCitizen(string phoneNumber);
    public Task<Result<AuthToken>> VerifyOtp(string otpToken, string code);
    public Task<Result<AuthToken>> Refresh(string token, string refreshToken);
    public Task<Result<bool>> Revoke(string userId, string refreshToken);
    public Task<Result<bool>> ChangePassword(string username, string oldPassword, string newPassword);
}

public record LoginResultModel(
    AuthToken? AuthToken,
    VerificationToken? VerificationToken);
public record AuthToken(string JwtToken, string RefreshToken);
public record VerificationToken(string PhoneNumber, string Token, string Code);