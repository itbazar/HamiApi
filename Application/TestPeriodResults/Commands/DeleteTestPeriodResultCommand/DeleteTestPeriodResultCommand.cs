using Domain.Models.Hami;
using MediatR;

namespace Application.TestPeriodResults.Commands.DeleteTestPeriodResultCommand;

public record DeleteTestPeriodResultCommand(
    Guid Id,
    bool? IsDeleted = true) : IRequest<Result<TestPeriodResult>>;
