using Application.Complaints.Common;
using Domain.Models.ComplaintAggregate;
using Domain.Primitives;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Persistence;

public interface IComplaintRepository
{
    //public Task<Result<bool>> Add(Complaint complaint);
    public Task<Result<bool>> Insert(Complaint complaint);
    public Task<Result<bool>> Update(Complaint complaint);
    public Complaint? GetComplaint(string trackingNumber);
    //public Task<Result<Complaint>> GetCitizenAsync(string trackingNumber, string password);
    //public Task<Result<Complaint>> GetInspectorAsync(string trackingNumber, string password);
    public Task<Result<Complaint>> GetAsync(string trackingNumber);
    public Task<Result<List<Complaint>>> GetAsync(Expression<Func<Complaint, bool>>? filter = null, int skip = 0, int take = 50, bool isTracking = true);
    public Task<Result<PagedList<Complaint>>> GetListCitizenAsync(PagingInfo pagingInfo, ComplaintListFilters filters, string userId);
    public Task<Result<PagedList<Complaint>>> GetListInspectorAsync(PagingInfo pagingInfo, ComplaintListFilters filters);
    //public Task<Result<bool>> ReplyInspector(Complaint complaint, string encodedKey);
    //public Task<Result<bool>> ReplyCitizen(Complaint complaint, string password);
    //public Task<Result<bool>> ChangeInspectorKey(string privateKey, Guid toKeyId, Guid? fromKeyId);
    public Task<Result<List<StatesHistogram>>> GetStatesHistogram();
    public Task<Result<long>> CountAsync(Expression<Func<Complaint, bool>>? filter = null);
}

public record StatesHistogram(ComplaintState State, int Count);