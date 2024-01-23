using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Mapster;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintListQuery;

internal class GetComplaintListCitizenQueryHandler : 
    IRequestHandler<GetComplaintListCitizenQuery, Result<PagedList<ComplaintListCitizenResponse>>>
{
    private readonly IComplaintRepository _complaintRepository;

    public GetComplaintListCitizenQueryHandler(IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<Result<PagedList<ComplaintListCitizenResponse>>> Handle(
        GetComplaintListCitizenQuery request, CancellationToken cancellationToken)
    {
        var complaintList = await _complaintRepository.GetListCitizenAsync(
            request.pagingInfo,
            request.Filters,
            request.UserId);
        if (complaintList.IsFailed)
            return complaintList.ToResult();

        var result = complaintList.Value.Adapt<List<ComplaintListCitizenResponse>>();
        return new PagedList<ComplaintListCitizenResponse>(
            result,
            complaintList.Value.Count,
            complaintList.Value.CurrentPage,
            complaintList.Value.PageSize);
    }
}
