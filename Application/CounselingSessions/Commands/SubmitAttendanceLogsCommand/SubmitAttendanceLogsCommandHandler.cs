using Application.Common.Interfaces.Persistence;
using Application.Complaints.Commands.Common;
using Domain.Models.Hami;
using Microsoft.AspNetCore.Mvc;

namespace Application.CounselingSessions.Commands.SubmitAttendanceLogsCommand;

internal class SubmitAttendanceLogsCommandHandler(
    ICounselingSessionRepository counselingSessionRepository,
    ISessionAttendanceLogRepository sessionAttendanceLogRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<SubmitAttendanceLogsCommand, Result<CounselingSession>>
{
    public async Task<Result<CounselingSession>> Handle(SubmitAttendanceLogsCommand request, CancellationToken cancellationToken)
    {
        var session = await counselingSessionRepository
           .GetAsync(q => q.Id == request.SessionId && !q.IsDeleted);
        foreach (var log in request.AttendanceLogs)
        {
            var attendanceLog = SessionAttendanceLog.Create(
                request.SessionId,
                log.UserId,
                log.Attended,
                log.MentorNote
            );

            sessionAttendanceLogRepository.Insert(attendanceLog);
        }
        await unitOfWork.SaveAsync();

       
        return session.FirstOrDefault();
    }
}