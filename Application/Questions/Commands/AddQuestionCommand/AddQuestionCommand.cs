using Domain.Models.Hami;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Questions.Commands.AddQuestionCommand;

public record AddQuestionCommand(
    TestType TestType,
    string QuestionText,
    bool? IsDeleted) : IRequest<Result<Question>>;
