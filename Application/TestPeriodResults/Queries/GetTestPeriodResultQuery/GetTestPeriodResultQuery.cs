using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.TestPeriodResults.Queries.GetTestPeriodResultQuery;

public record GetTestPeriodResultQuery(PagingInfo PagingInfo) : IRequest<Result<PagedList<TestPeriodResult>>>;
