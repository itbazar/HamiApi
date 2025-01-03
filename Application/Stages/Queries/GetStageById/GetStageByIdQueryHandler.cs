
using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;
using MediatR;
using FluentResults;
using Application.ComplaintCategories.Queries.GetComplaintCategoryByIdQuery;

namespace Application.Stages.Queries.GetStageById;

internal class GetStageByIdQueryHandler(IStageRepository diseaseRepository)
    : IRequestHandler<GetStageByIdQuery, Result<Stage>>
{
    public async Task<Result<Stage>> Handle(GetStageByIdQuery request, CancellationToken cancellationToken)
    {
        // دریافت Stage از دیتابیس
        var result = await diseaseRepository.GetSingleAsync(d => d.Id == request.Id && !d.IsDeleted);
        if (result is null)
            return GenericErrors.NotFound;

        return result;
    }
}