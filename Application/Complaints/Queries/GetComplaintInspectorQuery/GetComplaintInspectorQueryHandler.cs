using Application.Common.Interfaces.Persistence;
using Application.Complaints.Queries.Common;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Mapster;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintInspectorQuery;

internal class GetComplaintCitizenQueryHandler : IRequestHandler<GetComplaintInspectorQuery, ComplaintResponse>
{
    private readonly IComplaintRepository _complaintRepository;

    public GetComplaintCitizenQueryHandler(IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<ComplaintResponse> Handle(GetComplaintInspectorQuery request, CancellationToken cancellationToken)
    {
        var complaint = await _complaintRepository.GetInspectorAsync(request.TrackingNumber, request.EncodedKey);
        if (complaint.ShouldMarkedAsRead())
        {
            var complaintToUpdate = await _complaintRepository.GetAsync(request.TrackingNumber);
            complaint.AddContent(
                "",
                new List<Media>(),
                Actor.Inspector,
                ComplaintOperation.Open,
                ComplaintContentVisibility.Inspector);
            await _complaintRepository.ReplyInspector(complaintToUpdate, request.EncodedKey);
        }

        var result = complaint.Adapt<ComplaintResponse>();
        result.PossibleOperations.AddRange(complaint.GetPossibleOperations(Actor.Inspector));

        return result;
    }
}
