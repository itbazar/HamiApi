
using Domain.Models.StageAggregate;
using FluentResults;
using MediatR;

namespace Application.Stages.Commands.AddStage;

public record AddStageCommand(string Title, string Description) : IRequest<Result<Stage>>;