using Application.Common.Interfaces.Persistence;
using Domain.Models.PublicKeys;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Errors;

namespace Infrastructure.Persistence.Repositories;

public class PublicKeyRepository(ApplicationDbContext context) : IPublicKeyRepository
{
    public async Task<Result<bool>> Add(PublicKey publicKey)
    {
        context.Add(publicKey);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Result<bool>> Delete(Guid id)
    {
        var result = await Get(id);
        if (result.IsFailed)
            return result.ToResult();
        var publicKey = result.Value;
        if (await context.Complaint.AnyAsync(c => c.PublicKeyId == id))
            return PublicKeyErrors.InUsedKeyCannotBeDeleted;
        publicKey.IsDeleted = true;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Result<PublicKey>> Get(Guid id)
    {
        var publicKey = await context.PublicKey
            .Where(p => p.Id == id && p.IsDeleted == false)
            .SingleOrDefaultAsync();
        if (publicKey is null)
            return GenericErrors.NotFound;
        return publicKey;
    }

    public async Task<Result<PublicKey>> GetActive()
    {
        var publicKey = await context.PublicKey
            .Where(p => p.IsActive && p.IsDeleted == false)
            .SingleOrDefaultAsync();
        if (publicKey is null)
            return GenericErrors.NotFound;
        return publicKey;
    }

    public async Task<Result<List<PublicKey>>> GetAll()
    {
        var result = await context.PublicKey
            .Where(p => p.IsDeleted == false)
            .ToListAsync();
        return result;
    }

    public async Task<Result<bool>> SetActive(Guid id)
    {
        var result = await Get(id);
        if (result.IsFailed)
            return result.ToResult();
        var toSetKey = result.Value;
        if (toSetKey.IsActive)
            return true;

        result = await GetActive();
        if(result.IsFailed)
            return result.ToResult();
        var currentKey = result.Value;

        if (toSetKey.IsDeleted)
            return PublicKeyErrors.DeletedKeyCannotSetAsActive;

        toSetKey.IsActive = true;
        currentKey.IsActive = false;

        await context.SaveChangesAsync();
        return true;
    }

    public Task<Result<bool>> Update(Guid id, string key)
    {
        throw new NotImplementedException();
    }
}

