using Domain.Models.Hami;
using MediatR;

namespace Application.TestPeriodResults.Queries.GetTestPeriodResultByIdQuery;

public record GetTestPeriodResultByIdQuery(Guid Id) : IRequest<Result<TestPeriodResult>>;
