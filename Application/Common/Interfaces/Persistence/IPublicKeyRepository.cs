using Domain.Models.PublicKeys;

namespace Application.Common.Interfaces.Persistence;

public interface IPublicKeyRepository
{
    public Task<bool> Add(PublicKey publicKey);
    public Task<bool> Update(Guid id, string key);
    public Task<List<PublicKey>> GetAll();
    public Task<bool> Delete(Guid id);
    public Task<PublicKey> Get(Guid id);
}
