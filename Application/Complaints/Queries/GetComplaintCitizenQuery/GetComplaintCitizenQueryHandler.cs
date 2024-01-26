using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Domain.Models.ComplaintAggregate;
using Mapster;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintCitizenQuery;

internal class GetComplaintCitizenQueryHandler : IRequestHandler<GetComplaintCitizenQuery, Result<ComplaintCitizenResponse>>
{
    private readonly IComplaintRepository _complaintRepository;

    public GetComplaintCitizenQueryHandler(IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<Result<ComplaintCitizenResponse>> Handle(GetComplaintCitizenQuery request, CancellationToken cancellationToken)
    {
        var complaint = _complaintRepository.GetComplaint(request.TrackingNumber);
        if (complaint == null)
        {
            return ComplaintErrors.NotFound;
        }
        complaint.GetCitizen(request.Password);
        complaint.Contents.RemoveAll(cc => cc.Visibility == ComplaintContentVisibility.Inspector);
        
        var result = new ComplaintCitizenResponse(
            complaint.TrackingNumber,
            complaint.Title,
            complaint.Category.Adapt<ComplaintCategoryResponse>(),
            complaint.Status,
            complaint.RegisteredAt,
            complaint.LastChanged,
            complaint.Complaining,
            complaint.ComplaintOrganization.Adapt<ComplaintOrganizationResponse>(),
            complaint.Contents.Adapt<List<ComplaintContentResponse>>(),
            complaint.GetPossibleOperations(Actor.Citizen));

        await Task.CompletedTask;
        return result;
    }
}
