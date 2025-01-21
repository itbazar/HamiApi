using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;

namespace Application.CounselingSessions.Queries.GetSessionAttendanceLogsQuery;

internal class GetSessionAttendanceLogsQueryHandler(ICounselingSessionRepository counselingSessionRepository,
    ISessionAttendanceLogRepository sessionAttendanceLogRepository) : IRequestHandler<GetSessionAttendanceLogsQuery, Result<List<SessionAttendanceLog>>>
{
    public async Task<Result<List<SessionAttendanceLog>>> Handle(GetSessionAttendanceLogsQuery request, CancellationToken cancellationToken)
    {
        var logs = await sessionAttendanceLogRepository.GetAsync(
           q => q.CounselingSessionId == request.SessionId,
           includeProperties: "User");

        return logs.ToList();
    }
}