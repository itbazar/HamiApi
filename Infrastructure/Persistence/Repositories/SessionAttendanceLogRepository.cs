using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Infrastructure.Persistence.Repositories;

public class SessionAttendanceLogRepository : GenericRepository<SessionAttendanceLog>, ISessionAttendanceLogRepository
{
    public SessionAttendanceLogRepository(ApplicationDbContext context) : base(context)
    {
    }
}
