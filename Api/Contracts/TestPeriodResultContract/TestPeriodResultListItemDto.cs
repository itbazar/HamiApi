using Domain.Models.Common;
using Domain.Models.Hami;

namespace Api.Contracts.TestPeriodResultContract;

public record TestPeriodResultListItemDto(
    Guid Id,
    string UserId,
    TestType TestType,
    int TotalScore,
    Guid TestPeriodId,
    DateTime CreatedAt);