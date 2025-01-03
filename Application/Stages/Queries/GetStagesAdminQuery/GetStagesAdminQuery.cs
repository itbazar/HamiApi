
using Domain.Models.Hami;
using MediatR;
using FluentResults;

namespace Application.Stages.Queries.GetStagesAdminQuery;

public record GetStagesAdminQuery() : IRequest<Result<List<Stage>>>;