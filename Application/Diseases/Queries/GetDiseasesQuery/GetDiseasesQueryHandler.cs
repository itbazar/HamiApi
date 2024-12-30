using Application.Common.Interfaces.Persistence;
using Domain.Models.DiseaseAggregate;
using MediatR;
using FluentResults;

namespace Application.Diseases.Queries.GetDiseases;

internal class GetDiseasesQueryHandler(
    IDiseaseRepository diseaseRepository)
    : IRequestHandler<GetDiseasesQuery, Result<List<Disease>>>
{
    public async Task<Result<List<Disease>>> Handle(GetDiseasesQuery request, CancellationToken cancellationToken)
    {
        // واکشی لیست Diseases که حذف نشده‌اند
        var result = await diseaseRepository.GetAsync(d => !d.IsDeleted);
        return result.ToList();
    }
}
