using Application.Common.Interfaces.Persistence;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetPatients;

internal class GetPatientsQueryHandler(IUserRepository userRepository) : IRequestHandler<GetPatientsQuery, Result<PagedList<ApplicationUser>>>
{
    public async Task<Result<PagedList<ApplicationUser>>> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
    {
        var result = await userRepository
            .GetPagedPatientsAsync(request.PagingInfo,
             request.Status,request.CurrentUserId);
        return result;
    
    }
}

//var complaintList = await _complaintRepository.GetListInspectorAsync(
//            request.pagingInfo,
//            request.Filters);
//        if (complaintList.IsFailed)
//            return complaintList.ToResult();
//        var result = complaintList.Value.Adapt<List<ComplaintListInspectorResponse>>();
//        return new PagedList<ComplaintListInspectorResponse>(
//            result,
//            complaintList.Value.TotalCount,
//            complaintList.Value.CurrentPage,
//            complaintList.Value.PageSize);