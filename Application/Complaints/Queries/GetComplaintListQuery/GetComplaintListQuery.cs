using Application.Common.Interfaces.Persistence;
using Application.Complaints.Queries.Common;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintListQuery;

public record GetComplaintListQuery(
    PagingInfo pagingInfo,
    ComplaintListFilters Filters,
    string? UserId = null) : IRequest<List<ComplaintListResponse>>;