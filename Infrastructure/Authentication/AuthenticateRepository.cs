
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Authentication;

public class AuthenticateRepository : IAuthenticateRepository
{
    private readonly IDatabase _database;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    public AuthenticateRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _database = _connectionMultiplexer.GetDatabase();
    }
    public async Task<bool> DeleteOtpAsync(string otpId)
    {
        await _database.KeyDeleteAsync($"otp:{otpId}");
        return false;
    }

    public async Task<Otp?> GetOtpAsync(string otpId)
    {
        string? serialized = await _database.StringGetAsync($"otp:{otpId}");
        if (serialized is null)
            return null;
        var token = JsonSerializer.Deserialize<Otp>(serialized);
        return token;
    }

    public async Task<bool> InsertOtpAsync(Otp otp)
    {
        await _database.StringSetAsync(
            $"otp:{otp.Token}",
            JsonSerializer.Serialize(otp),
            TimeSpan.FromMinutes(5));
        return true;
    }

    public async Task<bool> InsertSentAsync(string username)
    {
        await _database.StringSetAsync(
            $"otp_sent:{username}",
            "",
            TimeSpan.FromMinutes(2));
        return true;
    }

    public async Task<bool> IsSentAsync(string username)
    {
        string? otp_sent = await _database.StringGetAsync($"otp_sent:{username}");
        return (otp_sent is not null);
    }

    public async Task<bool> DeleteRefreshTokenAsync(string tokenId)
    {
        await _database.KeyDeleteAsync($"ref:{tokenId}");
        return false;
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string tokenId)
    {
        string? serialized = await _database.StringGetAsync($"ref:{tokenId}");
        if (serialized is null)
            return null;
        var token = JsonSerializer.Deserialize<RefreshToken>(serialized);
        return token;
    }

    public async Task<bool> InsertRefreshTokenAsync(RefreshToken token)
    {
        await _database.StringSetAsync(
            $"ref:{token.Token}",
            JsonSerializer.Serialize(token),
            token.Expiry);
        return true;
    }

    public async Task<ResetPasswordToken?> GetResetPasswordTokenAsync(string userId)
    {
        string? serialized = await _database.StringGetAsync($"res:{userId}");
        if (serialized is null)
            return null;
        var token = JsonSerializer.Deserialize<ResetPasswordToken>(serialized);
        return token;
    }

    public async Task<bool> InsertResetPasswordTokenAsync(ResetPasswordToken resetPasswordToken)
    {
        await _database.StringSetAsync(
            $"res:{resetPasswordToken.UserId}",
            JsonSerializer.Serialize(resetPasswordToken),
            TimeSpan.FromMinutes(15));
        return true;
    }

    public async Task<bool> DeleteResetPasswordTokenAsync(string userId)
    {
        await _database.KeyDeleteAsync($"res:{userId}");
        return false;
    }
}