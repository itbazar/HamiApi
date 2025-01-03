using Domain.Models.ComplaintAggregate;
using Domain.Models.Hami;

namespace Application.Common.Interfaces.Persistence;

public interface ITestPeriodResultRepository 
{
    public Task<Result<bool>> Insert(TestPeriodResult info);

    public Task<Result<bool>> Update(TestPeriodResult info);
}
