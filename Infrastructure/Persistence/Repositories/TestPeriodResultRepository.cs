using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Infrastructure.Persistence.Repositories;

public class TestPeriodResultRepository : GenericRepository<TestPeriodResult>, ITestPeriodResultRepository
{
    public TestPeriodResultRepository(ApplicationDbContext context) : base(context)
    {
    }
}


