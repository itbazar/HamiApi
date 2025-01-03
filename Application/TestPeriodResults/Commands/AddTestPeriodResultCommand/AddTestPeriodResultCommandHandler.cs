using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.TestPeriodResults.Commands.AddTestPeriodResultCommand;

internal class AddTestPeriodResultCommandHandler(
    ITestPeriodResultRepository testPeriodResultRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddTestPeriodResultCommand, Result<TestPeriodResult>>
{
    public async Task<Result<TestPeriodResult>> Handle(AddTestPeriodResultCommand request, CancellationToken cancellationToken)
    {
        var testPeriodResult = TestPeriodResult.Create(
             request.UserId,
            request.TestType,
            request.TotalScore,
            request.TestPeriodId
        ); 
        testPeriodResultRepository.Insert(testPeriodResult);
        await unitOfWork.SaveAsync();
        return testPeriodResult;
    }
}