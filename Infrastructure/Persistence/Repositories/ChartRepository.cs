using Application.Common.Interfaces.Persistence;
using Domain.Models.ChartAggregate;

namespace Infrastructure.Persistence.Repositories;

public class ChartRepository : GenericRepository<Chart>, IChartRepository
{
    public ChartRepository(ApplicationDbContext context) : base(context)
    {
    }
}
