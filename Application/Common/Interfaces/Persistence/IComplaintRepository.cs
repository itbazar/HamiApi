using Application.Complaints.Common;
using Domain.Models.ComplaintAggregate;

namespace Application.Common.Interfaces.Persistence;

public interface IComplaintRepository
{
    public Task<Result<bool>> Add(Complaint complaint);
    public Task<Result<Complaint>> GetCitizenAsync(string trackingNumber, string password);
    public Task<Result<Complaint>> GetInspectorAsync(string trackingNumber, string password);
    public Task<Result<Complaint>> GetAsync(string trackingNumber);
    public Task<Result<List<Complaint>>> GetListCitizenAsync(PagingInfo pagingInfo, ComplaintListFilters filters, string userId);
    public Task<Result<List<Complaint>>> GetListInspectorAsync(PagingInfo pagingInfo, ComplaintListFilters filters);
    public Task<Result<bool>> ReplyInspector(Complaint complaint, string encodedKey);
    public Task<Result<bool>> ReplyCitizen(Complaint complaint, string password);
    public Task<Result<bool>> ChangeInspectorKey(string privateKey, Guid toKeyId, Guid? fromKeyId);
}
