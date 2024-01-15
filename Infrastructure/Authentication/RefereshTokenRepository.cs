
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Authentication;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IDatabase _database;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    public RefreshTokenRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _database = _connectionMultiplexer.GetDatabase();
    }
    public async Task<bool> DeleteAsync(string tokenId)
    {
        await _database.KeyDeleteAsync($"ref:{tokenId}");
        return false;
    }

    public async Task<RefreshToken?> GetAsync(string tokenId)
    {
        string? serialized = await _database.StringGetAsync($"ref:{tokenId}");
        if (serialized is null)
            return null;
        var token = JsonSerializer.Deserialize<RefreshToken>(serialized);
        return token;
    }

    public async Task<bool> InsertAsync(RefreshToken token)
    {
        await _database.StringSetAsync(
            $"ref:{token.Token}",
            JsonSerializer.Serialize(token),
            token.Expiry);
        return true;
    }
}