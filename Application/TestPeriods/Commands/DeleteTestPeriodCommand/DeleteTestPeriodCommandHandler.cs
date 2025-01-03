using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.TestPeriods.Commands.DeleteTestPeriodCommand;

internal class DeleteTestPeriodCommandHandler(
    ITestPeriodRepository testPeriodrRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteTestPeriodCommand, Result<TestPeriod>>
{
    public async Task<Result<TestPeriod>> Handle(DeleteTestPeriodCommand request, CancellationToken cancellationToken)
    {
        var testPeriodr = await testPeriodrRepository.GetSingleAsync(s => s.Id == request.Id);
        if (testPeriodr is null)
            return GenericErrors.NotFound;
        testPeriodr.Delete(request.IsDeleted.Value);
        testPeriodrRepository.Update(testPeriodr);
        await unitOfWork.SaveAsync();
        return testPeriodr;
    }
}