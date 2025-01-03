using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Infrastructure.Persistence.Repositories;

public class CounselingSessionRepository : GenericRepository<CounselingSession>, ICounselingSessionRepository
{
    public CounselingSessionRepository(ApplicationDbContext context) : base(context)
    {
    }
}
