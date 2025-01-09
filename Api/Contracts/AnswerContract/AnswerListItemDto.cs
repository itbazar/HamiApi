using Domain.Models.Common;
using Domain.Models.Hami;

namespace Api.Contracts.AnswerContract;

public record AnswerListItemDto(
    Guid Id,
    string UserId,
    Guid QuestionId,
    int? AnswerValue,
    Guid TestPeriodId,
    DateTime AnswerDate);