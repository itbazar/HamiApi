using Application.Common.Interfaces.Persistence;
using Domain.Models.ChartAggregate;
using MediatR;

namespace Application.Charts.Queries.GetChartByIdQuery;

internal class GetComplaintCategoryByIdQueryHandler : IRequestHandler<GetChartByIdQuery, Chart>
{
    private readonly IChartRepository _chartRepository;

    public GetComplaintCategoryByIdQueryHandler(IChartRepository chartRepository)
    {
        _chartRepository = chartRepository;
    }

    public async Task<Chart> Handle(GetChartByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _chartRepository.GetSingleAsync(cc => cc.IsDeleted == false);
        if (result is null)
            throw new Exception("Not found!");
        return result;
    }
}
