using Domain.Models.Hami;
using MediatR;

namespace Application.TestPeriodApp.Queries.GetTestPeriodByIdQuery;

public record GetTestPeriodByIdQuery(Guid Id) : IRequest<Result<TestPeriod>>;
