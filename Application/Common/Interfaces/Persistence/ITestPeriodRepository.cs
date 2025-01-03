using Domain.Models.ComplaintAggregate;
using Domain.Models.Hami;

namespace Application.Common.Interfaces.Persistence;

public interface ITestPeriodRepository : IGenericRepository<TestPeriod>
{
    public Task<Result<TestPeriod>> GetAsyncByCode(int code);
}
