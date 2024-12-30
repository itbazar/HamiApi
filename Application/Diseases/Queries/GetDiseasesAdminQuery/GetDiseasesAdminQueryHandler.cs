using Application.Common.Interfaces.Persistence;
using Domain.Models.DiseaseAggregate;
using MediatR;
using FluentResults;

namespace Application.Diseases.Queries.GetDiseasesAdminQuery;

internal class GetDiseasesAdminQueryHandler(
    IDiseaseRepository diseaseRepository)
    : IRequestHandler<GetDiseasesAdminQuery, Result<List<Disease>>>
{
    public async Task<Result<List<Disease>>> Handle(GetDiseasesAdminQuery request, CancellationToken cancellationToken)
    {
        // واکشی لیست Diseases از مخزن
        var result = await diseaseRepository.GetAsync();
        return result.ToList();
    }
}
