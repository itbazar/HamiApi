using Domain.Models.IdentityAggregate;

namespace Application.Common.Interfaces.Security;

public interface IAuthenticationService
{
    public Task<Result<LoginResultModel>> Login(string username, string password, bool twoFactorEnabled = false);
    public Task<Result<VerificationToken>> LogisterCitizen(string phoneNumber);
    public Task<Result<string>> PreRegisterPatient(string phoneNumber);
    public Task<Result<AuthToken>> VerifyOtp(string otpToken, string code);
    public Task<Result<AuthToken>> Refresh(string token, string refreshToken);
    public Task<Result<bool>> Revoke(string userId, string refreshToken);
    public Task<Result<bool>> ChangePassword(string username, string oldPassword, string newPassword);
    public Task<Result<RequestToChangePhoneNumberResult>> RequestToChangePhoneNumber(
        string userName, string newPhoneNumber);
    public Task<Result<bool>> ChangePhoneNumber(
        string userName, string otpToken1, string code1, string otpToken2, string code2);

    public Task<Result<VerificationToken>> GetResetPasswordToken(string username);
    public Task<Result<bool>> ResetPassword(string username, string resetPasswordToken, string newPassword);
    public Task<Result<VerificationToken>> ResendVerificationCode(string otpToken);
}

public record LoginResultModel(
    AuthToken? AuthToken,
    VerificationToken? VerificationToken);
public record AuthToken(string JwtToken, string RefreshToken);
public record VerificationToken(string PhoneNumber, string Token, string Code);
public record RequestToChangePhoneNumberResult(VerificationToken Token1, VerificationToken Token2);