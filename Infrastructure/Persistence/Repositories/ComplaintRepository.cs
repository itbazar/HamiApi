using Application.Common.Interfaces.Encryption;
using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Domain.Models.ComplaintAggregate;
using FluentResults;
using Infrastructure.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Errors;
using System.Security.Cryptography;

namespace Infrastructure.Persistence.Repositories;

public class ComplaintRepository(
    ApplicationDbContext context,
    ISymmetricEncryption symmetric,
    IAsymmetricEncryption asymmetric,
    IHasher hasher) : IComplaintRepository
{
    public async Task<Result<bool>> Add(Complaint complaint)
    {
        complaint.TrackingNumber = GenerateTrackingNumber();

        complaint.GenerateCredentials(hasher, symmetric, asymmetric);

        if (complaint.Contents.Count != 1)
            return ComplaintErrors.InconsistentContent;

        complaint.EncryptContent(hasher, symmetric);

        context.Add(complaint);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Result<Complaint>> GetCitizenAsync(string trackingNumber, string password)
    {
        password = password.Trim();
        var complaint = getComplaint(trackingNumber);
        await Task.CompletedTask;

        if (complaint is null)
            return ComplaintErrors.NotFound;

        complaint.LoadEncryptionKeyByCitizenPassword(password, hasher, symmetric);
        complaint.DecryptContent(hasher, symmetric);
        return complaint;
    }

    public async Task<Result<Complaint>> GetInspectorAsync(string trackingNumber, string encodedKey)
    {
        var complaint = getComplaint(trackingNumber);
        await Task.CompletedTask;

        if (complaint is null)
            return ComplaintErrors.NotFound;

        complaint.LoadEncryptionKeyByInspector(encodedKey, hasher);
        complaint.DecryptContent(hasher, symmetric);
        return complaint;
    }

    public async Task<Result<Complaint>> GetAsync(string trackingNumber)
    {
        var complaint = await context.Complaint
            .Where(c => c.TrackingNumber == trackingNumber)
            .SingleOrDefaultAsync();
        if (complaint is null)
            return ComplaintErrors.NotFound;
        return complaint;
    }

    public async Task<Result<PagedList<Complaint>>> GetListCitizenAsync(PagingInfo pagingInfo, ComplaintListFilters filters, string userId)
    {
        var query = context.Complaint.Where(c => c.UserId == userId);

        if(filters.States is not null && filters.States.Count > 0)
            query = query.Where(c => filters.States.Contains(c.Status));

        if(filters.TrackingNumber is not null && !filters.TrackingNumber.IsNullOrEmpty())
            query = query.Where(c => c.TrackingNumber.Contains(filters.TrackingNumber));

        query = query
            .Include(c => c.Category)
            .Include(c => c.ComplaintOrganization);
        var complaintList = await PagedList<Complaint>.ToPagedList(query, pagingInfo.PageNumber, pagingInfo.PageSize);
        
        return complaintList;
    }

    public async Task<Result<PagedList<Complaint>>> GetListInspectorAsync(PagingInfo pagingInfo, ComplaintListFilters filters)
    {
        var query = context.Complaint.Where(c => true);

        if (filters.States is not null && filters.States.Count > 0)
            query = query.Where(c => filters.States.Contains(c.Status));

        if (filters.TrackingNumber is not null && !filters.TrackingNumber.IsNullOrEmpty())
            query = query.Where(c => c.TrackingNumber.Contains(filters.TrackingNumber));

        query = query
            .Include(c => c.Category)
            .Include(c => c.ComplaintOrganization);
        var complaintList = await PagedList<Complaint>.ToPagedList(query, pagingInfo.PageNumber, pagingInfo.PageSize);
        
        return complaintList;
    }

    public async Task<Result<bool>> ReplyInspector(Complaint complaint, string encodedKey)
    {
        complaint.LoadEncryptionKeyByInspector(encodedKey, hasher);
        complaint.EncryptContent(hasher, symmetric);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Result<bool>> ReplyCitizen(Complaint complaint, string password)
    {
        password = password.Trim();
        complaint.LoadEncryptionKeyByCitizenPassword(password, hasher, symmetric);
        complaint.EncryptContent(hasher, symmetric);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Result<bool>> ChangeInspectorKey(string privateKey, Guid toKeyId, Guid? fromKeyId)
    {
        //TODO: Consider using transactions and improve the performance by pagination
        var publicKeys = await context.PublicKey.ToListAsync();
        fromKeyId = fromKeyId ?? publicKeys.Where(p => p.IsActive).Select(p => p.Id).SingleOrDefault();
        publicKeys.ForEach(p => p.IsActive = false);
        var fromPublicKey = publicKeys.Where(p => fromKeyId == null || p.Id == fromKeyId).SingleOrDefault();
        var toPublicKey = publicKeys.Where(p => p.Id == toKeyId).SingleOrDefault();
        if (fromPublicKey is null || toPublicKey is null)
            return ComplaintErrors.PublicKeyNotFound;
        toPublicKey.IsActive = true;

        //TODO: Consider pagination
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
            .Include(c => c.User)
            .Include(c => c.ComplaintOrganization)
            .Include(c => c.Category)
            .Include(c => c.Contents)
            .ThenInclude(cc => cc.Media)
            .AsNoTracking()
            .SingleOrDefault();
    }

    public async Task<Result<List<StatesHistogram>>> GetStatesHistogram()
    {
        var result = await context.Complaint
            .GroupBy(c => c.Status)
            .Select(g => new StatesHistogram(g.Key, g.Count()))
            .ToListAsync();
        return result;
    }
}
