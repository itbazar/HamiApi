namespace Infrastructure.Authentication;

public interface IAuthenticateRepository
{
    public Task<Otp?> GetOtpAsync(string token);
    public Task<bool> InsertOtpAsync(Otp token);
    public Task<bool> DeleteOtpAsync(string token);
    public Task<bool> InsertSentAsync(string username);
    public Task<bool> IsSentAsync(string username);
    public Task<RefreshToken?> GetRefreshTokenAsync(string token);
    public Task<bool> InsertRefreshTokenAsync(RefreshToken refreshToken);
    public Task<bool> DeleteRefreshTokenAsync(string token);
    public Task<ResetPasswordToken?> GetResetPasswordTokenAsync(string userId);
    public Task<bool> InsertResetPasswordTokenAsync(ResetPasswordToken resetPasswordToken);
    public Task<bool> DeleteResetPasswordTokenAsync(string userId);
}