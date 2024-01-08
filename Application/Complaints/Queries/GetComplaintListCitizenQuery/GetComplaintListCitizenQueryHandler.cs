using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Mapster;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintListQuery;

internal class GetComplaintListCitizenQueryHandler : 
    IRequestHandler<GetComplaintListCitizenQuery, List<ComplaintListCitizenResponse>>
{
    private readonly IComplaintRepository _complaintRepository;

    public GetComplaintListCitizenQueryHandler(IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<List<ComplaintListCitizenResponse>> Handle(
        GetComplaintListCitizenQuery request, CancellationToken cancellationToken)
    {
        var complaintList = await _complaintRepository.GetListInspectorAsync(
            request.pagingInfo,
            request.Filters);
        return complaintList.Adapt<List<ComplaintListCitizenResponse>>();
    }
}
