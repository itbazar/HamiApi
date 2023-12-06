using Application.Common.Interfaces.Persistence;
using Application.Complaints.Queries.Common;
using Mapster;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintListQuery;

internal class GetComplaintListQueryHandler : IRequestHandler<GetComplaintListQuery, List<ComplaintListInspectorResponse>>
{
    private readonly IComplaintRepository _complaintRepository;

    public GetComplaintListQueryHandler(IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<List<ComplaintListInspectorResponse>> Handle(GetComplaintListQuery request, CancellationToken cancellationToken)
    {
        var complaintList = await _complaintRepository.GetListAsync(request.pagingInfo);
        return complaintList.Adapt<List<ComplaintListInspectorResponse>>();
    }
}
