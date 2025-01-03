
using MediatR;
using FluentResults;
using Domain.Models.Hami;

namespace Application.Stages.Commands.DeleteStage;
public record DeleteStageCommand(Guid Id, bool IsDeleted) : IRequest<Result<Stage>>;