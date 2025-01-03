using Domain.Models.Hami;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Questions.Commands.UpdateQuestionCommand;

public record UpdateQuestionCommand(
    Guid Id,
    TestType? TestType,
    string? QuestionText,
    bool? IsDeleted) : IRequest<Result<Question>>;
