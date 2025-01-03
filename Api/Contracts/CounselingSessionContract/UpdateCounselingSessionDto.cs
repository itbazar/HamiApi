namespace Api.Contracts.CounselingSessionContract;

public record UpdateCounselingSessionDto(
   string? MentorId,
    DateTime? ScheduledDate,
    string? Topic,
    string? MeetingLink,
    string? MentorNote);
