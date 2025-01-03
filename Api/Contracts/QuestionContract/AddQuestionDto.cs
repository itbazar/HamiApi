using Domain.Models.Hami;

namespace Api.Contracts.QuestionContract;

public record AddQuestionDto(
   TestType TestType,
    string QuestionText="",
    bool IsDeleted=false);