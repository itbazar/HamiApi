using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.TestPeriods.Queries.GetTestPeriodQuery;

public record GetTestPeriodQuery(PagingInfo PagingInfo) : IRequest<Result<PagedList<TestPeriod>>>;
