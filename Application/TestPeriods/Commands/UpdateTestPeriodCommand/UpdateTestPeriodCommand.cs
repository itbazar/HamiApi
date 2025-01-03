using Domain.Models.Hami;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.TestPeriods.Commands.UpdateTestPeriodCommand;

public record UpdateTestPeriodCommand(
    Guid Id,
    TestType? TestType,
   string? PeriodName,
        DateTime? StartDate,
        DateTime? EndDate,
        int? Code) : IRequest<Result<TestPeriod>>;
