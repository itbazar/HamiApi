using Domain.Models.Hami;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Answers.Commands.AddAnswerCommand;

public record AddAnswerCommand(
    string UserId,
    Guid QuestionId,
    int AnswerValue,
    Guid TestPeriodId) : IRequest<Result<Answer>>;
