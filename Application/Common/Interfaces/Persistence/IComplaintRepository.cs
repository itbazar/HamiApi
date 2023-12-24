using Application.Complaints.Common;
using Domain.Models.ComplaintAggregate;

namespace Application.Common.Interfaces.Persistence;

public interface IComplaintRepository
{
    public Task<bool> Add(Complaint complaint);
    public Task<Complaint> GetCitizenAsync(string trackingNumber, string password);
    public Task<Complaint> GetInspectorAsync(string trackingNumber, string password);
    public Task<Complaint> GetAsync(string trackingNumber);
    public Task<List<Complaint>> GetListAsync(PagingInfo pagingInfo, ComplaintListFilters filters, string? userId);
    public Task<bool> ReplyInspector(Complaint complaint, string encodedKey);
    public Task<bool> ReplyCitizen(Complaint complaint, string password);
    public Task<bool> ChangeInspectorKey(string privateKey, Guid toKeyId, Guid? fromKeyId);
}
