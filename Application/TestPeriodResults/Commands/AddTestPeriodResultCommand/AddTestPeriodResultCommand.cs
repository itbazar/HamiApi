using Domain.Models.Hami;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.TestPeriodResults.Commands.AddTestPeriodResultCommand;

public record AddTestPeriodResultCommand(
    string UserId,
    TestType TestType,
    int TotalScore,
    Guid TestPeriodId,
    int TestInstance) : IRequest<Result<TestPeriodResult>>;
