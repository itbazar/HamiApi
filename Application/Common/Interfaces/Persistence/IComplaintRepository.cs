using Domain.Models.ComplaintAggregate;

namespace Application.Common.Interfaces.Persistence;

public interface IComplaintRepository
{
    public bool Add(Complaint complaint);
    public Task<Complaint> GetAsync(string trackingNumber, string password, Actor actor);
}
