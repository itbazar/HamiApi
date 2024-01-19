using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Mapster;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintInspectorQuery;

internal class ComplaintInspectorResponseHandler : 
    IRequestHandler<GetComplaintInspectorQuery, Result<ComplaintInspectorResponse>>
{
    private readonly IComplaintRepository _complaintRepository;

    public ComplaintInspectorResponseHandler(IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<Result<ComplaintInspectorResponse>> Handle(GetComplaintInspectorQuery request, CancellationToken cancellationToken)
    {
        var complaint = await _complaintRepository.GetInspectorAsync(request.TrackingNumber, request.EncodedKey);
        if (complaint.ShouldMarkedAsRead())
        {
            var complaintToUpdate = await _complaintRepository.GetAsync(request.TrackingNumber);
            complaintToUpdate.AddContent(
                "",
                new List<Media>(),
                Actor.Inspector,
                ComplaintOperation.Open,
                ComplaintContentVisibility.Inspector);
            await _complaintRepository.ReplyInspector(complaintToUpdate, request.EncodedKey);
            complaint = await _complaintRepository.GetInspectorAsync(request.TrackingNumber, request.EncodedKey);
        }

        var result = new ComplaintInspectorResponse(
            complaint.TrackingNumber,
            complaint.Title,
            complaint.Category.Adapt<ComplaintCategoryResponse>(),
            complaint.Status,
            complaint.RegisteredAt,
            complaint.LastChanged,
            complaint.Complaining,
            complaint.ComplaintOrganization.Adapt<ComplaintOrganizationResponse>(),
            complaint.Contents.Adapt<List<ComplaintContentResponse>>(),
            complaint.GetPossibleOperations(Actor.Inspector),
            complaint.User.Adapt<UserResponse>());

        return result;
    }
}
