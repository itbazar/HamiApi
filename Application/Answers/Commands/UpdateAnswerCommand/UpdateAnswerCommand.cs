using Domain.Models.Hami;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Answers.Commands.UpdateAnswerCommand;

public record UpdateAnswerCommand(
    Guid Id,
    int AnswerValue) : IRequest<Result<Answer>>;
