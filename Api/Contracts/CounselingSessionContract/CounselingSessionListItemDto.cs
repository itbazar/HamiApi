using Domain.Models.Common;

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
    string MentorNote);