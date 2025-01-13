using Domain.Models.Common;
using Domain.Models.Hami;

namespace Api.Contracts.CounselingSessionContract;

public record CounselingSessionListItemDto(
    Guid Id,
    Guid PatientGroupId,
    string PatientGroupName,
    string MentorId,
    string MentorName, // اضافه کردن نام کاربر
    DateTime ScheduledDate,
    string Topic,
    string MeetingLink,
    string MentorNote,
    bool? AttendanceStatus);


public record SubmitAttendanceLogsDto(Guid SessionId, List<SessionAttendanceLog> AttendanceLogs);

public record AttendanceLogDto(string UserId,string UserName,string FirstName, string LastName, bool Attended, string? MentorNote);