using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;

namespace Application.Complaints.Queries.GetComplaintInspectorQuery;

internal class ComplaintInspectorResponseHandler(IComplaintRepository complaintRepository) : 
    IRequestHandler<GetComplaintInspectorQuery, Result<ComplaintInspectorResponse>>
{
    public async Task<Result<ComplaintInspectorResponse>> Handle(
        GetComplaintInspectorQuery request,
        CancellationToken cancellationToken)
    {
        var complaint = complaintRepository.GetComplaint(request.TrackingNumber);
        
        if (complaint == null)
        {
            return ComplaintErrors.NotFound;
        }

        if (complaint.ShouldMarkedAsRead())
        {
            complaint = (await complaintRepository.GetAsync(request.TrackingNumber)).Value;
            complaint.MarkAsRead(request.EncodedKey);
            await complaintRepository.Update(complaint);
        }

        var result = ComplaintInspectorResponse.FromComplaint(complaint);

        await Task.CompletedTask;
        return result;
    }
}
