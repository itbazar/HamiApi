
namespace Application.Stages.Commands.EditStage;

using MediatR;
using FluentResults;
using Domain.Models.StageAggregate;

public record EditStageCommand(Guid Id, string Title, string Description) : IRequest<Result<Stage>>;