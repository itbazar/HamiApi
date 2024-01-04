using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Mapster;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintListQuery;

internal class GetComplaintListQueryHandler : IRequestHandler<GetComplaintListQuery, List<ComplaintListResponse>>
{
    private readonly IComplaintRepository _complaintRepository;

    public GetComplaintListQueryHandler(IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<List<ComplaintListResponse>> Handle(GetComplaintListQuery request, CancellationToken cancellationToken)
    {
        var complaintList = await _complaintRepository.GetListAsync(
            request.pagingInfo,
            request.Filters,
            request.UserId);
        return complaintList.Adapt<List<ComplaintListResponse>>();
    }
}
