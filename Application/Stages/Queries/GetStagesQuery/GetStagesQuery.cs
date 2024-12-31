
using Application.Stages.Common;
using Domain.Models.StageAggregate;

namespace Application.Stages.Queries.GetStages;

public record GetStagesQuery : IRequest<Result<List<Stage>>>;