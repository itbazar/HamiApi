using Application.Common.Interfaces.Encryption;
using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Domain.Models.ComplaintAggregate;
using Infrastructure.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Infrastructure.Persistence.Repositories;

public class ComplaintRepository(
    ApplicationDbContext context,
    ISymmetricEncryption symmetric,
    IAsymmetricEncryption asymmetric,
    IHasher hasher,
    IPublicKeyRepository publicKeyRepository) : IComplaintRepository
{
    public async Task<bool> Add(Complaint complaint)
    {
        complaint.TrackingNumber = GenerateTrackingNumber();

        complaint.GenerateCredentials(hasher, symmetric, asymmetric);
        
        if (complaint.Contents.Count != 1)
            throw new Exception("Inconsistent contents.");

        complaint.EncryptContent(hasher, symmetric);

        context.Add(complaint);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Complaint> GetCitizenAsync(string trackingNumber, string password)
    {
        var complaint = getComplaint(trackingNumber);
        await Task.CompletedTask;

        if (complaint is null)
            throw new Exception("Not found");

        complaint.LoadEncryptionKeyByCitizenPassword(password, hasher, symmetric);
        complaint.DecryptContent(hasher, symmetric);
        return complaint;
    }

    public async Task<Complaint> GetInspectorAsync(string trackingNumber, string encodedKey)
    {
        var complaint = getComplaint(trackingNumber);
        await Task.CompletedTask;

        if (complaint is null)
            throw new Exception("Not found");

        complaint.LoadEncryptionKeyByInspector(encodedKey, hasher);
        complaint.DecryptContent(hasher, symmetric);
        return complaint;
    }

    public async Task<Complaint> GetAsync(string trackingNumber)
    {
        var complaint = await context.Complaint
            .Where(c => c.TrackingNumber == trackingNumber)
            .SingleOrDefaultAsync();
        return complaint ?? throw new Exception("Not found.");
    }

    public async Task<List<Complaint>> GetListAsync(PagingInfo pagingInfo, ComplaintListFilters filters, string? userId = null)
    {
        var query = context.Complaint.Where(c => true);
        if(userId is not null)
            query = query.Where(c => c.UserId == userId);

        if(filters.States is not null && filters.States.Count > 0)
            query = query.Where(c => filters.States.Contains(c.Status));

        if(filters.TrackingNumber is not null && !filters.TrackingNumber.IsNullOrEmpty())
            query = query.Where(c => c.TrackingNumber.Contains(filters.TrackingNumber));

        var complaintList = await query
            .Skip(pagingInfo.PageSize * (pagingInfo.PageNumber -1))
            .Take(pagingInfo.PageSize)
            .ToListAsync();
        return complaintList;
    }

    public async Task<bool> ReplyInspector(Complaint complaint, string encodedKey)
    {
        complaint.LoadEncryptionKeyByInspector(encodedKey, hasher);
        complaint.EncryptContent(hasher, symmetric);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ReplyCitizen(Complaint complaint, string password)
    {
        complaint.LoadEncryptionKeyByCitizenPassword(password, hasher, symmetric);
        complaint.EncryptContent(hasher, symmetric);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ChangeInspectorKey(string privateKey, Guid toKeyId, Guid? fromKeyId)
    {
        //TODO: Consider using transactions and improve the performance by pagination
        var publicKeys = await context.PublicKey.ToListAsync();
        publicKeys.ForEach(p => p.IsActive = false);
        var fromPublicKey = publicKeys.Where(p => fromKeyId == null || p.Id == fromKeyId).SingleOrDefault();
        var toPublicKey = publicKeys.Where(p => p.Id == toKeyId).SingleOrDefault();
        if (fromPublicKey is null || toPublicKey is null)
            throw new Exception("Public key not found.");
        toPublicKey.IsActive = true;

        var complaints = await context.Set<Complaint>().Where(c => c.PublicKeyId == fromKeyId).ToListAsync();
        foreach (var complaint in complaints)
        {
            complaint.ChangeInspectorKey(privateKey, toPublicKey.Key, hasher, asymmetric);
            complaint.PublicKeyId = toKeyId;
        }

        await context.SaveChangesAsync();
        return true;
    }

    private string GenerateTrackingNumber()
    {
        return RandomNumberGenerator.GetInt32(10000000, 99999999).ToString();
    }

    private Complaint? getComplaint(string trackingNumber)
    {
        return context.Complaint
            .Where(c => c.TrackingNumber == trackingNumber)
            .Include(c => c.Contents)
            .ThenInclude(cc => cc.Media)
            .AsNoTracking()
            .SingleOrDefault();
    }
}
