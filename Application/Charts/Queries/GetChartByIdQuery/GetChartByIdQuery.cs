using Domain.Models.ChartAggregate;
using MediatR;

namespace Application.Charts.Queries.GetChartByIdQuery;

public record GetChartByIdQuery(Guid Id) : IRequest<Chart>;
