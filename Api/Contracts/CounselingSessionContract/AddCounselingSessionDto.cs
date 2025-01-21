namespace Api.Contracts.CounselingSessionContract;

public record AddCounselingSessionDto(
    Guid PatientGroupId,
    DateTime ScheduledDate,
    string MentorId = "",
    string Topic ="",
    string MeetingLink = "",
    string MentorNote = "");