using Application.Charts.Queries.GetChartByIdQuery;
using Application.Common.Interfaces.Persistence;
using Domain.Models.ChartAggregate;
using Mapster;
using MediatR;

namespace Application.Charts.Queries.GetChartsQuery;

internal class GetChartsQueryHandler : IRequestHandler<GetChartsQuery, List<ChartResponse>>
{
    private readonly IChartRepository _chartRepository;

    public GetChartsQueryHandler(IChartRepository chartRepository)
    {
        _chartRepository = chartRepository;
    }

    public async Task<List<ChartResponse>> Handle(GetChartsQuery request, CancellationToken cancellationToken)
    {
        var result = await _chartRepository.GetAsync(
            cc => cc.IsDeleted == false && request.RoleNames.Any(rn => cc.Roles.Any(cr => cr.Name == rn)),
            false,
            cc => cc.OrderBy(o => o.Order));

        return result.ToList().Adapt<List<ChartResponse>>();
    }
}
