using Domain.Models.Hami;

namespace Api.Contracts.QuestionContract;

public record UpdateQuestionDto(
    TestType? TestType,
    string? QuestionText,
    bool? IsDeleted);
