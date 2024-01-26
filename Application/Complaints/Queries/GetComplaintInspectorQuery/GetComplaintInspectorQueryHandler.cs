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
        var complaint = _complaintRepository.GetComplaint(request.TrackingNumber);
        if (complaint == null)
        {
            return ComplaintErrors.NotFound;
        }
        if (!complaint.GetInspector(request.EncodedKey))
        {
            await _complaintRepository.Update(complaint);
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

        await Task.CompletedTask;
        return result;
    }
}
