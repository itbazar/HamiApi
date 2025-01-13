using Domain.Models.Hami;

namespace Api.Contracts.TestPeriodResultContract;

public record AddTestPeriodResultDto(
    string? UserId,
    TestType TestType,
    int TotalScore,
    Guid TestPeriodId);