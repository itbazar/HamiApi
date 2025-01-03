using Domain.Models.ComplaintAggregate;
using Domain.Models.Hami;

namespace Application.Common.Interfaces.Persistence;

public interface ITestPeriodRepository 
{
    public Task<Result<bool>> Insert(TestPeriod info);

    public Task<Result<bool>> Update(TestPeriod info);
    public Task<Result<TestPeriod>> GetAsyncByCode(int code);
}
