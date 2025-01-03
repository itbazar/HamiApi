using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.TestPeriodApp.Queries.GetTestPeriodByIdQuery;

internal class GetTestPeriodByIdQueryHandler : IRequestHandler<GetTestPeriodByIdQuery, Result<TestPeriod>>
{
    private readonly ITestPeriodRepository _testPeriodRepository;

    public GetTestPeriodByIdQueryHandler(ITestPeriodRepository testPeriodRepository)
    {
        _testPeriodRepository = testPeriodRepository;
    }

    public async Task<Result<TestPeriod>> Handle(GetTestPeriodByIdQuery request, CancellationToken cancellationToken)
    {
        var testPeriod = await _testPeriodRepository.GetSingleAsync(s => s.Id == request.Id && s.IsDeleted != true, false);
        if (testPeriod is null)
            return GenericErrors.NotFound;
        return testPeriod;
    }
}