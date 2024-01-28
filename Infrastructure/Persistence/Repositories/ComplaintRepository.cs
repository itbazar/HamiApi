using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Domain.Models.ComplaintAggregate;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Errors;

namespace Infrastructure.Persistence.Repositories;

public class ComplaintRepository(
    ApplicationDbContext context) : IComplaintRepository
{
    public async Task<Result<bool>> Insert(Complaint complaint)
    {
        context.Add(complaint);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Result<bool>> Update(Complaint complaint)
    {
        context.Update(complaint);

        await context.SaveChangesAsync();
        return true;
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

    public async Task<Result<PagedList<Complaint>>> GetListCitizenAsync(
        PagingInfo pagingInfo,
        ComplaintListFilters filters,
        string userId)
    {
        var query = context.Complaint.Where(c => c.UserId == userId);

        if(filters.States is not null && filters.States.Count > 0)
            query = query.Where(c => filters.States.Contains(c.Status));

        if(filters.TrackingNumber is not null && !filters.TrackingNumber.IsNullOrEmpty())
            query = query.Where(c => c.TrackingNumber.Contains(filters.TrackingNumber));

        query = query
            .Include(c => c.Category)
            .Include(c => c.ComplaintOrganization)
            .OrderByDescending(c => c.LastChanged);
        var complaintList = await PagedList<Complaint>.ToPagedList(query, pagingInfo.PageNumber, pagingInfo.PageSize);
        
        return complaintList;
    }

    public async Task<Result<PagedList<Complaint>>> GetListInspectorAsync(
        PagingInfo pagingInfo,
        ComplaintListFilters filters)
    {
        var query = context.Complaint.Where(c => true);

        if (filters.States is not null && filters.States.Count > 0)
            query = query.Where(c => filters.States.Contains(c.Status));

        if (filters.TrackingNumber is not null && !filters.TrackingNumber.IsNullOrEmpty())
            query = query.Where(c => c.TrackingNumber.Contains(filters.TrackingNumber));

        query = query
            .Include(c => c.Category)
            .Include(c => c.ComplaintOrganization)
            .OrderByDescending(c => c.LastChanged);
        var complaintList = await PagedList<Complaint>.ToPagedList(query, pagingInfo.PageNumber, pagingInfo.PageSize);
        
        return complaintList;
    }

    public Complaint? GetComplaint(string trackingNumber)
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
