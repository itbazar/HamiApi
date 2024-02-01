using Domain.Models.PublicKeys;

namespace Application.Common.Interfaces.Persistence;

public interface IPublicKeyRepository
{
    public Task<Result<bool>> Add(PublicKey publicKey);
    public Task<Result<bool>> Update(Guid id, string key);
    public Task<Result<List<PublicKey>>> GetAll();
    public Task<Result<bool>> Delete(Guid id);
    public Task<Result<PublicKey>> Get(Guid id);
    public Task<Result<PublicKey>> GetActive();
    public Task<Result<bool>> SetActive(Guid id);
}
