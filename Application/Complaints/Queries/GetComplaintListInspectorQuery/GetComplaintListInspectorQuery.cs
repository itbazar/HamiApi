using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintListQuery;

public record GetComplaintListInspectorQuery(
    PagingInfo pagingInfo,
    ComplaintListFilters Filters) : IRequest<Result<PagedList<ComplaintListInspectorResponse>>>;