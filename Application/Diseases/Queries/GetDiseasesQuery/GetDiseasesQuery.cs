using Application.Diseases.Common;
using Domain.Models.Hami;

namespace Application.Diseases.Queries.GetDiseases;

public record GetDiseasesQuery : IRequest<Result<List<Disease>>>;
