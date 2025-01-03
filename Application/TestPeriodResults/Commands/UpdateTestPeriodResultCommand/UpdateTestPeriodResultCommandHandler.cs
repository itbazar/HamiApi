using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.Hami;
using Infrastructure.Storage;

namespace Application.TestPeriodResults.Commands.UpdateTestPeriodResultCommand;

internal class UpdateTestPeriodResultCommandHandler(
    ITestPeriodResultRepository testPeriodResultRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateTestPeriodResultCommand, Result<TestPeriodResult>>
{
    public async Task<Result<TestPeriodResult>> Handle(UpdateTestPeriodResultCommand request, CancellationToken cancellationToken)
    {
        var testPeriodResult = await testPeriodResultRepository.GetSingleAsync(s => s.Id == request.Id);
        if (testPeriodResult is null)
            throw new Exception("Not found.");
        testPeriodResult.Update(request.TotalScore);

        testPeriodResultRepository.Update(testPeriodResult);
        await unitOfWork.SaveAsync();
        return testPeriodResult;
    }
}