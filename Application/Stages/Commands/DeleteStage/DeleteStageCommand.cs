
using MediatR;
using FluentResults;
using Domain.Models.StageAggregate;

namespace Application.Stages.Commands.DeleteStage;
public record DeleteStageCommand(Guid Id, bool IsDeleted) : IRequest<Result<Stage>>;