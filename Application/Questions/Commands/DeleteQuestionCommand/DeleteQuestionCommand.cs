using Domain.Models.Hami;
using MediatR;

namespace Application.Questions.Commands.DeleteQuestionCommand;

public record DeleteQuestionCommand(
    Guid Id,
    bool? IsDeleted = null) : IRequest<Result<Question>>;
