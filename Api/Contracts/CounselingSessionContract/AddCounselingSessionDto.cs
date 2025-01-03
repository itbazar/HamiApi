namespace Api.Contracts.CounselingSessionContract;

public record AddCounselingSessionDto(
    Guid PatientGroupId,
    string MentorId,
    DateTime ScheduledDate,
    string Topic="",
    string MeetingLink = "",
    string MentorNote = "");