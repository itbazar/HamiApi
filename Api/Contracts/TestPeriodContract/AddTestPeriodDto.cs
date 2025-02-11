using Domain.Models.Hami;

namespace Api.Contracts.TestPeriodContract;

public record AddTestPeriodDto(
    TestType TestType,
        string PeriodName,
        DateTime StartDate,
        DateTime EndDate,
        int Code,
        RecurrenceType Recurrence);