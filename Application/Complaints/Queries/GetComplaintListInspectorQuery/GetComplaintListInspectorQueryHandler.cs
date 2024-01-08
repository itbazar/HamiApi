using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Mapster;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintListQuery;

internal class GetComplaintListInspectorQueryHandler : 
    IRequestHandler<GetComplaintListInspectorQuery, List<ComplaintListInspectorResponse>>
{
    private readonly IComplaintRepository _complaintRepository;

    public GetComplaintListInspectorQueryHandler(IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<List<ComplaintListInspectorResponse>> Handle(
        GetComplaintListInspectorQuery request, CancellationToken cancellationToken)
    {
        var complaintList = await _complaintRepository.GetListInspectorAsync(
            request.pagingInfo,
            request.Filters);
        return complaintList.Adapt<List<ComplaintListInspectorResponse>>();
    }
}
