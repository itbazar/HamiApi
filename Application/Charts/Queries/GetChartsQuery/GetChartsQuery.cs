using Domain.Models.ChartAggregate;
using MediatR;

namespace Application.Charts.Queries.GetChartsQuery;

public record GetChartsQuery(List<string> RoleNames) : IRequest<List<Chart>>;
