using Application.Common.Interfaces.Persistence;
using Application.Complaints.Queries.Common;
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
        var result = await _complaintRepository.GetCitizenAsync(request.TrackingNumber, request.Password);
        return result.Adapt<ComplaintResponse>();
    }
}
