using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.TestPeriods.Commands.AddTestPeriodCommand;

internal class AddTestPeriodCommandHandler(
    ITestPeriodRepository testPeriodRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddTestPeriodCommand, Result<TestPeriod>>
{
    public async Task<Result<TestPeriod>> Handle(AddTestPeriodCommand request, CancellationToken cancellationToken)
    {
        var testPeriod = TestPeriod.Create(
            request.TestType,
            request.PeriodName,
            request.StartDate,
            request.EndDate,
            request.Code
        ); 
        testPeriodRepository.Insert(testPeriod);
        await unitOfWork.SaveAsync();
        return testPeriod;
    }
}