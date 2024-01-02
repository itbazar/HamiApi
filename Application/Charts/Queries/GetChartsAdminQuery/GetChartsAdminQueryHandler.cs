using Application.Common.Interfaces.Persistence;
using Domain.Models.ChartAggregate;
using MediatR;

namespace Application.Charts.Queries.GetChartsAdminQuery;

internal class GetChartsAdminQueryHandler : IRequestHandler<GetChartsAdminQuery, List<Chart>>
{
    private readonly IChartRepository _categoryRepository;

    public GetChartsAdminQueryHandler(IChartRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<Chart>> Handle(GetChartsAdminQuery request, CancellationToken cancellationToken)
    {
        var result = await _categoryRepository.GetAsync();
        return result.ToList();
    }
}
