using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.TestPeriods.Queries.GetTestPeriodQuery;

internal class GetTestPeriodQueryHandler : IRequestHandler<GetTestPeriodQuery, Result<PagedList<TestPeriod>>>
{
    private readonly ITestPeriodRepository _testPeriodRepository;

    public GetTestPeriodQueryHandler(ITestPeriodRepository testPeriodRepository)
    {
        _testPeriodRepository = testPeriodRepository;
    }

    public async Task<Result<PagedList<TestPeriod>>> Handle(GetTestPeriodQuery request, CancellationToken cancellationToken)
    {
        var testPeriod = await _testPeriodRepository.GetPagedAsync(
            request.PagingInfo,
            s => s.IsDeleted == false &&
                 s.TestType != TestType.MOOD && // فیلتر برای عدم نمایش تست مود
                 s.Code != 101 && s.Code != 102, // فیلتر برای عدم نمایش کدهای 101 و 102
            false,
            s => s.OrderByDescending(o => o.StartDate)); // مرتب‌سازی بر اساس تاریخ شروع به صورت نزولی

        return testPeriod;
    }

}