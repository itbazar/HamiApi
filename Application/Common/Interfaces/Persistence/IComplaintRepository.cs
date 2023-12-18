using Domain.Models.ComplaintAggregate;

namespace Application.Common.Interfaces.Persistence;

public interface IComplaintRepository
{
    public Task<bool> Add(Complaint complaint);
    public Task<Complaint> GetCitizenAsync(string trackingNumber, string password);
    public Task<Complaint> GetInspectorAsync(string trackingNumber, string password);
    public Task<Complaint> GetAsync(string trackingNumber);
    public Task<List<Complaint>> GetListAsync(PagingInfo pagingInfo);
    public Task<bool> ReplyInspector(Complaint complaint, string encryptedKey);
    public Task<bool> ReplyCitizen(Complaint complaint, string password);
}
