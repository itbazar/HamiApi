using Application.Common.Interfaces.Persistence;
using Application.Complaints.Queries.Common;
using Domain.Models.ComplaintAggregate;
using Mapster;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintCitizenQuery;

internal class GetComplaintCitizenQueryHandler : IRequestHandler<GetComplaintCitizenQuery, ComplaintResponse>
{
    private readonly IComplaintRepository _complaintRepository;

    public GetComplaintCitizenQueryHandler(IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<ComplaintResponse> Handle(GetComplaintCitizenQuery request, CancellationToken cancellationToken)
    {
        var complaint = await _complaintRepository.GetCitizenAsync(request.TrackingNumber, request.Password);
        complaint.Contents.RemoveAll(cc => cc.Visibility == ComplaintContentVisibility.Inspector);
        var result = complaint.Adapt<ComplaintResponse>();
        result.PossibleOperations.AddRange(complaint.GetPossibleOperations(Actor.Citizen));
        return result;
    }
}
