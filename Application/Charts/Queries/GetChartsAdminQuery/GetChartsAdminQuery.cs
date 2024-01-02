using Domain.Models.ChartAggregate;
using MediatR;

namespace Application.Charts.Queries.GetChartsAdminQuery;

public record GetChartsAdminQuery() : IRequest<List<Chart>>;
