
using Application.Common.Interfaces.Persistence;
using Domain.Models.StageAggregate;

namespace Application.Stages.Queries.GetStagesAdminQuery;

internal class GetStagesAdminQueryHandler(
    IStageRepository stageRepository)
    : IRequestHandler<GetStagesAdminQuery, Result<List<Stage>>>
{
    public async Task<Result<List<Stage>>> Handle(GetStagesAdminQuery request, CancellationToken cancellationToken)
    {
        // Fetch list of stages from the repository
        var result = await stageRepository.GetAsync();
        
        // Return as a result list
        return Result.Ok(result.ToList());
    }
}