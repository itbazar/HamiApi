using Domain.Models.Hami;
using MediatR;

namespace Application.TestPeriods.Commands.DeleteTestPeriodCommand;

public record DeleteTestPeriodCommand(
    Guid Id,
    bool? IsDeleted = true) : IRequest<Result<TestPeriod>>;
