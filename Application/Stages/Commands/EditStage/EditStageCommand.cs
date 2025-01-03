
namespace Application.Stages.Commands.EditStage;

using MediatR;
using FluentResults;
using Domain.Models.Hami;

public record EditStageCommand(Guid Id, string Title, string Description) : IRequest<Result<Stage>>;