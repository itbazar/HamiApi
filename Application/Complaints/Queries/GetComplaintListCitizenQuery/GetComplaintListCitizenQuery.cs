using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintListQuery;

public record GetComplaintListCitizenQuery(
    PagingInfo pagingInfo,
    ComplaintListFilters Filters,
    string UserId) : IRequest<Result<PagedList<ComplaintListCitizenResponse>>>;