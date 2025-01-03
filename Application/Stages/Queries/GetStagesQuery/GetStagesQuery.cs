
using Application.Stages.Common;
using Domain.Models.Hami;

namespace Application.Stages.Queries.GetStages;

public record GetStagesQuery : IRequest<Result<List<Stage>>>;