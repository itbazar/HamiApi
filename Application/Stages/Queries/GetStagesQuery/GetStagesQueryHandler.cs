
using Application.Common.Interfaces.Persistence;
using Domain.Models.StageAggregate;
using MediatR;
using FluentResults;

namespace Application.Stages.Queries.GetStages;

internal class GetStagesQueryHandler(
    IStageRepository diseaseRepository)
    : IRequestHandler<GetStagesQuery, Result<List<Stage>>>
{
    public async Task<Result<List<Stage>>> Handle(GetStagesQuery request, CancellationToken cancellationToken)
    {
        // ????? ???? Stages ?? ??? ????????
        var result = await diseaseRepository.GetAsync(d => !d.IsDeleted);
        return result.ToList();
    }
}