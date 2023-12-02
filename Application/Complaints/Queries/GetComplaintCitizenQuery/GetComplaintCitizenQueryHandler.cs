using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintCitizenQuery;

internal class GetComplaintCitizenQueryHandler : IRequestHandler<GetComplaintCitizenQuery, Complaint>
{
    private readonly IComplaintRepository _complaintRepository;

    public GetComplaintCitizenQueryHandler(IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<Complaint> Handle(GetComplaintCitizenQuery request, CancellationToken cancellationToken)
    {
        var result = await _complaintRepository.GetAsync(request.TrackingNumber, request.Password, Actor.Citizen);
        return result;
    }
}
