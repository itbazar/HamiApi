using Domain.Models.Hami;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.TestPeriodResults.Commands.UpdateTestPeriodResultCommand;

public record UpdateTestPeriodResultCommand(
    Guid Id,
    int? TotalScore) : IRequest<Result<TestPeriodResult>>;
