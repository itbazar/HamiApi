using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.Hami;
using Infrastructure.Storage;

namespace Application.TestPeriods.Commands.UpdateTestPeriodCommand;

internal class UpdateTestPeriodCommandHandler(
    ITestPeriodRepository testPeriodRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateTestPeriodCommand, Result<TestPeriod>>
{
    public async Task<Result<TestPeriod>> Handle(UpdateTestPeriodCommand request, CancellationToken cancellationToken)
    {
        var testPeriod = await testPeriodRepository.GetSingleAsync(s => s.Id == request.Id);
        if (testPeriod is null)
            throw new Exception("Not found.");
        testPeriod.Update(
            request.TestType,
            request.PeriodName,
            request.StartDate,
            request.EndDate,
            request.Code);

        testPeriodRepository.Update(testPeriod);
        await unitOfWork.SaveAsync();
        return testPeriod;
    }
}