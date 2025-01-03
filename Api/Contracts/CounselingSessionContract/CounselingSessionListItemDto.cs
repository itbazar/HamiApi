using Domain.Models.Common;

namespace Api.Contracts.CounselingSessionContract;

public record CounselingSessionListItemDto(
    Guid PatientGroupId,
    string MentorId,
    DateTime ScheduledDate,
    string Topic,
    string MeetingLink,
    string MentorNote);