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
            
            var markAsReadResult = complaint.MarkAsRead(request.EncodedKey);
            if (markAsReadResult.IsFailed)
                return markAsReadResult;

            await complaintRepository.Update(complaint);
            complaint = complaintRepository.GetComplaint(request.TrackingNumber)!;
        }
        var getResult = complaint.GetInspector(request.EncodedKey);
        if (getResult.IsFailed)
        {
            return getResult;
        }
        var result = ComplaintInspectorResponse.FromComplaint(complaint);

        return result;
    }
}
