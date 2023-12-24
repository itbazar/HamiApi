using Application.Common.Interfaces.Persistence;
using Domain.Models.PublicKeys;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class PublicKeyRepository : IPublicKeyRepository
{
    private ApplicationDbContext _context;

    public PublicKeyRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<bool> Add(PublicKey publicKey)
    {
        _context.Add(publicKey);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(Guid id)
    {
        var publicKey = await Get(id);
        publicKey.IsActive = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PublicKey> Get(Guid id)
    {
        var publicKey = await _context.Set<PublicKey>().Where(p => p.Id == id).SingleOrDefaultAsync();
        if (publicKey is null)
        {
            throw new Exception("Not found.");
        }
        return publicKey;
    }

    public async Task<List<PublicKey>> GetAll()
    {
        var result = await _context.Set<PublicKey>().ToListAsync();
        return result;
    }

    public Task<bool> Update(Guid id, string key)
    {
        throw new NotImplementedException();
    }
}

