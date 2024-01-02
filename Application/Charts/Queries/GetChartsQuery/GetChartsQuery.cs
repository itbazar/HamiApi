using Domain.Models.ChartAggregate;
using MediatR;

namespace Application.Charts.Queries.GetChartsQuery;

public record GetChartsQuery() : IRequest<List<Chart>>;
