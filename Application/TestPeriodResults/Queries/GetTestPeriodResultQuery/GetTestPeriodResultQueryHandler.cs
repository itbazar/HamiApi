using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.TestPeriodResults.Queries.GetTestPeriodResultQuery;

internal class GetTestPeriodResultQueryHandler : IRequestHandler<GetTestPeriodResultQuery, Result<PagedList<TestPeriodResult>>>
{
    private readonly ITestPeriodResultRepository _testPeriodResultRepository;

    public GetTestPeriodResultQueryHandler(ITestPeriodResultRepository testPeriodResultRepository)
    {
        _testPeriodResultRepository = testPeriodResultRepository;
    }

    public async Task<Result<PagedList<TestPeriodResult>>> Handle(GetTestPeriodResultQuery request, CancellationToken cancellationToken)
    {
        var testPeriodResult = await _testPeriodResultRepository.GetPagedAsync(
         request.PagingInfo,
         filter: s => !s.IsDeleted,
         trackChanges: false,
         orderBy: s => s.OrderByDescending(o => o.CreatedAt),
         includeProperties: "User,TestPeriod" // بارگذاری User و TestPeriod
         );

        return testPeriodResult;
    }

}