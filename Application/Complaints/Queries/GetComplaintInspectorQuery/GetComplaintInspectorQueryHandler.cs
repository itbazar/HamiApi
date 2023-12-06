using Application.Common.Interfaces.Persistence;
using Application.Complaints.Queries.Common;
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
        var result = await _complaintRepository.GetInspectorAsync(request.TrackingNumber, request.Password);
        return result.Adapt<ComplaintResponse>();
    }
}
