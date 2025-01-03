using Domain.Models.Hami;

namespace Api.Contracts.TestPeriodResultContract;

public record UpdateTestPeriodResultDto(
    string? UserId,
    TestType? TestType,
    int? TotalScore,
    Guid? TestPeriodId);
