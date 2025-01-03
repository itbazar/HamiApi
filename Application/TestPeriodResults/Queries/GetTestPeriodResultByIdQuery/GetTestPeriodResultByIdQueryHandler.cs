using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.TestPeriodResults.Queries.GetTestPeriodResultByIdQuery;

internal class GetTestPeriodResultByIdQueryHandler : IRequestHandler<GetTestPeriodResultByIdQuery, Result<TestPeriodResult>>
{
    private readonly ITestPeriodResultRepository _testPeriodResultRepository;

    public GetTestPeriodResultByIdQueryHandler(ITestPeriodResultRepository testPeriodResultRepository)
    {
        _testPeriodResultRepository = testPeriodResultRepository;
    }

    public async Task<Result<TestPeriodResult>> Handle(GetTestPeriodResultByIdQuery request, CancellationToken cancellationToken)
    {
        var testPeriodResult = await _testPeriodResultRepository.GetSingleAsync(s => s.Id == request.Id && s.IsDeleted != true, false);
        if (testPeriodResult is null)
            return GenericErrors.NotFound;
        return testPeriodResult;
    }
}