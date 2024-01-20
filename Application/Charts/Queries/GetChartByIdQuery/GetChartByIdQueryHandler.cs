using Application.Common.Interfaces.Persistence;
using Mapster;

namespace Application.Charts.Queries.GetChartByIdQuery;

internal class GetComplaintCategoryByIdQueryHandler(IChartRepository chartRepository) : IRequestHandler<GetChartByIdQuery, Result<ChartResponse>>
{
    public async Task<Result<ChartResponse>> Handle(GetChartByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await chartRepository.GetSingleAsync(cc => cc.IsDeleted == false);
        if (result is null)
            return GenericErrors.NotFound;
        return result.Adapt<ChartResponse>();
    }
}
