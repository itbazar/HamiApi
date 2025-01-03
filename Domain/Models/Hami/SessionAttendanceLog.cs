using Domain.Primitives;
using Domain.Models.IdentityAggregate;

namespace Domain.Models.Hami;

public class SessionAttendanceLog : Entity
{
    private SessionAttendanceLog(Guid id) : base(id) { }

    // متد ایجاد
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
    public CounselingSession CounselingSession { get; set; } = null!; 

    public string? UserId { get; set; }
    public ApplicationUser User { get; set; } = null!; 

    public bool Attended { get; set; } // آیا شرکت کرده است؟
    public string? MentorNote { get; set; } // یادداشت منتور
}
