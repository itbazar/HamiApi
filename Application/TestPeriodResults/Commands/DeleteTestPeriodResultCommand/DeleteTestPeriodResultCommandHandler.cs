using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.TestPeriodResults.Commands.DeleteTestPeriodResultCommand;

internal class DeleteTestPeriodResultCommandHandler(
    ITestPeriodResultRepository testPeriodResultrRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteTestPeriodResultCommand, Result<TestPeriodResult>>
{
    public async Task<Result<TestPeriodResult>> Handle(DeleteTestPeriodResultCommand request, CancellationToken cancellationToken)
    {
        var testPeriodResultr = await testPeriodResultrRepository.GetSingleAsync(s => s.Id == request.Id);
        if (testPeriodResultr is null)
            return GenericErrors.NotFound;
        testPeriodResultr.Delete(request.IsDeleted.Value);
        testPeriodResultrRepository.Update(testPeriodResultr);
        await unitOfWork.SaveAsync();
        return testPeriodResultr;
    }
}