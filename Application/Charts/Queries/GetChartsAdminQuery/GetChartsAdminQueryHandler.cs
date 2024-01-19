using Application.Charts.Queries.GetChartByIdQuery;
using Application.Common.Interfaces.Persistence;
using Domain.Models.ChartAggregate;
using Mapster;
using MediatR;

namespace Application.Charts.Queries.GetChartsAdminQuery;

internal class GetChartsAdminQueryHandler(IChartRepository categoryRepository) : IRequestHandler<GetChartsAdminQuery, Result<List<ChartResponse>>>
{
    public async Task<Result<List<ChartResponse>>> Handle(GetChartsAdminQuery request, CancellationToken cancellationToken)
    {
        var result = await categoryRepository.GetAsync();
        return result.ToList().Adapt<List<ChartResponse>>();
    }
}
