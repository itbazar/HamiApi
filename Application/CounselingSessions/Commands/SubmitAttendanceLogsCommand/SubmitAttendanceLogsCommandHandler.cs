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
        // پیدا کردن جلسه
        var session = await counselingSessionRepository
           .GetAsync(q => q.Id == request.SessionId && !q.IsDeleted);

        var updateSession = session.FirstOrDefault();
        //if (updateSession == null)
        //{
        //    return Result.Fail<CounselingSession>("Session not found.");
        //}

        // ثبت حضور و غیاب
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

        // به‌روزرسانی وضعیت جلسه
        updateSession.Update(
            updateSession.ScheduledDate,
            updateSession.Topic,
            updateSession.MeetingLink,
            updateSession.MentorNote,
            true // مقدار IsConfirmed
        );

        counselingSessionRepository.Update(updateSession);

        // ذخیره در دیتابیس
        await unitOfWork.SaveAsync();

        return updateSession;
    }
}
