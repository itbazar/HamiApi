using Domain.Models.DiseaseAggregate;
using MediatR;
using FluentResults;

namespace Application.Diseases.Queries.GetDiseasesAdminQuery;

public record GetDiseasesAdminQuery() : IRequest<Result<List<Disease>>>;
