
namespace Infrastructure.Persistence.Repositories;

using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

public class StageRepository : GenericRepository<Stage>, IStageRepository
{
    public StageRepository(ApplicationDbContext context) : base(context)
    {
    }
}
