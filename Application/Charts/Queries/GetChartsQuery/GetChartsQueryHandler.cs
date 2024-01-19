using Application.Charts.Queries.GetChartByIdQuery;
using Application.Common.Interfaces.Persistence;
using Mapster;
using MediatR;

namespace Application.Charts.Queries.GetChartsQuery;

internal class GetChartsQueryHandler(IChartRepository chartRepository) : IRequestHandler<GetChartsQuery, Result<List<ChartResponse>>>
{
    public async Task<Result<List<ChartResponse>>> Handle(GetChartsQuery request, CancellationToken cancellationToken)
    {
        var result = await chartRepository.GetAsync(
            cc => cc.IsDeleted == false && request.RoleNames.Any(rn => cc.Roles.Any(cr => cr.Name == rn)),
            false,
            cc => cc.OrderBy(o => o.Order));

        return result.ToList().Adapt<List<ChartResponse>>();
    }
}
