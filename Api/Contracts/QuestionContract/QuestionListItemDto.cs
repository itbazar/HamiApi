using Domain.Models.Common;
using Domain.Models.Hami;

namespace Api.Contracts.QuestionContract;

public record QuestionListItemDto(
    Guid Id,
    TestType TestType,
    string QuestionText,
    bool IsDeleted);