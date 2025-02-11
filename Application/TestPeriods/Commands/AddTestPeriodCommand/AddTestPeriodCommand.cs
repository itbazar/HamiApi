using Domain.Models.Hami;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.TestPeriods.Commands.AddTestPeriodCommand;

public record AddTestPeriodCommand(
    TestType TestType,
        string PeriodName,
        DateTime StartDate,
        DateTime EndDate,
        int Code,
        RecurrenceType Recurrence) : IRequest<Result<TestPeriod>>;
