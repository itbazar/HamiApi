namespace Application.Common.Interfaces.Security;

public interface IAuthenticationService
{
    public Task<LoginResultModel> Login(string username, string password, string? verificationCode = null);
    public Task<LoginResultModel> Refresh(string token, string refreshToken);
    public Task<LoginResultModel> LogisterCitizen(string phoneNumber, string? verificationCode);
    public Task<bool> ChangePassword(string username, string oldPassword, string newPassword);
    public Task<VerificationCodeModel> GetVerificationCode(string username);
    public Task<bool> VerifyPhoneNumber(string username, string verificationCode);
    public Task<string> GetResetPasswordToken(string username, string verificationCode);
    public Task<bool> ResetPassword(string username, string resetPasswordToken,  string newPassword);

}

public record LoginResultModel(string JwtToken, string RefreshToken, bool UserNotConfirmed = false);
public record VerificationCodeModel(string PhoneNumber, string Code);