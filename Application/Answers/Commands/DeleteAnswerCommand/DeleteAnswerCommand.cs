using Domain.Models.Hami;
using MediatR;

namespace Application.Answers.Commands.DeleteAnswerCommand;

public record DeleteAnswerCommand(
    Guid Id,
    bool? IsDeleted = null) : IRequest<Result<Answer>>;
