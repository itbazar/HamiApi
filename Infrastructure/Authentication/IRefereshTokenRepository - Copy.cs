namespace Infrastructure.Authentication;

public interface IRefreshTokenRepository
{
    public Task<RefreshToken?> GetAsync(string token);
    public Task<bool> InsertAsync(RefreshToken token);
    public Task<bool> DeleteAsync(string token);
}