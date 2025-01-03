using Domain.Models.Hami;

namespace Api.Contracts.TestPeriodContract;

public record UpdateTestPeriodDto(
    TestType? TestType,
        string? PeriodName,
        DateTime? StartDate,
        DateTime? EndDate,
        int? Code);
