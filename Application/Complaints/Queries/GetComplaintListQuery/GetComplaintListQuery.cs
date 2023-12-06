using Application.Common.Interfaces.Persistence;
using Application.Complaints.Queries.Common;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintListQuery;

public record GetComplaintListQuery(PagingInfo pagingInfo) : IRequest<List<ComplaintListInspectorResponse>>;