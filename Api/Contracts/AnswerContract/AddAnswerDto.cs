using Domain.Models.Hami;

namespace Api.Contracts.AnswerContract;

public record AddAnswerDto(
    string UserId,
    Guid QuestionId,
    int? AnswerValue,
    Guid TestPeriodId);