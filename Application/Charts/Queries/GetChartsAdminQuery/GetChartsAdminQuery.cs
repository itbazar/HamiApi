using Application.Charts.Queries.GetChartByIdQuery;
using MediatR;

namespace Application.Charts.Queries.GetChartsAdminQuery;

public record GetChartsAdminQuery() : IRequest<Result<List<ChartResponse>>>;
