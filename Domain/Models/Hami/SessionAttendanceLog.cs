using Domain.Models.IdentityAggregate;
using Domain.Primitives;

public class SessionAttendanceLog : Entity
{
    private SessionAttendanceLog(Guid id) : base(id) { }

    public static SessionAttendanceLog Create(Guid counselingSessionId, string userId, bool attended, string? mentorNote)
    {
        return new SessionAttendanceLog(Guid.NewGuid())
        {
            CounselingSessionId = counselingSessionId,
            UserId = userId,
            Attended = attended,
            MentorNote = mentorNote
        };
    }

    public void Update(bool? attended, string? mentorNote)
    {
        Attended = attended ?? Attended;
        MentorNote = mentorNote ?? MentorNote;
    }

    public Guid CounselingSessionId { get; set; }

    // جلوگیری از چرخه مرجع
    [System.Text.Json.Serialization.JsonIgnore]
    public CounselingSession CounselingSession { get; set; } = null!;

    public string? UserId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore] // جلوگیری از چرخه مرجع
    public ApplicationUser User { get; set; } = null!;

    public bool Attended { get; set; }
    public string? MentorNote { get; set; }
}
