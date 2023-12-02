namespace Application.Common.Interfaces.Security;

public interface IAuthenticationService
{
    public Task<LoginResultModel> Login(string username, string password, string? verificationCode = null);
    public Task<bool> RegisterCitizen(string username, string password);
    public Task<bool> ChangePassword(string username, string oldPassword, string newPassword);
    public Task<string> GetVerificationCode(string username);
    public Task<bool> VerifyPhoneNumber(string username, string verificationCode);
    public Task<string> GetResetPasswordToken(string username, string verificationCode);
    public Task<bool> ResetPassword(string username, string resetPasswordToken,  string newPassword);

}

public record LoginResultModel(string JwtToken, bool UserNotConfirmed = false);
