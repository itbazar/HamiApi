using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Mapster;

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
        var complaintResult = await _complaintRepository.GetInspectorAsync(request.TrackingNumber, request.EncodedKey);
        if (complaintResult.IsFailed)
            return complaintResult.ToResult();
        var complaint = complaintResult.Value;
        if (complaint.ShouldMarkedAsRead())
        {
            var complaintToUpdateResult = await _complaintRepository.GetAsync(request.TrackingNumber);
            if(complaintToUpdateResult.IsFailed)
                return complaintToUpdateResult.ToResult();

            var complaintToUpdate = complaintToUpdateResult.Value;
            try
            {
                complaintToUpdate.AddContent(
                "",
                new List<Media>(),
                Actor.Inspector,
                ComplaintOperation.Open,
                ComplaintContentVisibility.Inspector);
            }
            catch
            {
                return ComplaintErrors.InvalidOperation;
            }
            
            await _complaintRepository.ReplyInspector(complaintToUpdate, request.EncodedKey);
            complaintResult = await _complaintRepository.GetInspectorAsync(request.TrackingNumber, request.EncodedKey);
            if (complaintResult.IsFailed)
                return complaintResult.ToResult();
            complaint = complaintResult.Value;
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
