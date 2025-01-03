using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;
using MediatR;
using FluentResults;
using Application.ComplaintCategories.Queries.GetComplaintCategoryByIdQuery;

namespace Application.Diseases.Queries.GetDiseaseById;

internal class GetDiseaseByIdQueryHandler(IDiseaseRepository diseaseRepository)
    : IRequestHandler<GetDiseaseByIdQuery, Result<Disease>>
{
    public async Task<Result<Disease>> Handle(GetDiseaseByIdQuery request, CancellationToken cancellationToken)
    {
        // دریافت Disease از دیتابیس
        var result = await diseaseRepository.GetSingleAsync(d => d.Id == request.Id && !d.IsDeleted);
        if (result is null)
            return GenericErrors.NotFound;

        return result;
    }
}
