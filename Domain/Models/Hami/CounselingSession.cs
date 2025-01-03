using Domain.Models.IdentityAggregate;
using Domain.Primitives;

namespace Domain.Models.Hami;

public class CounselingSession : Entity
{
    private CounselingSession(Guid id) : base(id) { }

    public static CounselingSession Create(Guid patientGroupId, string mentorId, DateTime scheduledDate, string topic, string meetingLink, string? mentorNote = null)
    {
        return new CounselingSession(Guid.NewGuid())
        {
            PatientGroupId = patientGroupId,
            MentorId = mentorId,
            ScheduledDate = scheduledDate,
            Topic = topic,
            MeetingLink = meetingLink,
            MentorNote = mentorNote
        };
    }

    public void Update(DateTime? scheduledDate, string? topic, string? meetingLink, string? mentorNote)
    {
        ScheduledDate = scheduledDate ?? ScheduledDate;
        Topic = topic ?? Topic;
        MeetingLink = meetingLink ?? MeetingLink;
        MentorNote = mentorNote ?? MentorNote;
    }

    public void Delete(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }

    public Guid PatientGroupId { get; set; } // کلید خارجی به گروه بیماران
    public PatientGroup PatientGroup { get; set; } = null!; // ارتباط با گروه
    public DateTime ScheduledDate { get; set; } // تاریخ و زمان جلسه
    public string Topic { get; set; } = string.Empty; // موضوع جلسه مشاوره
    public string MeetingLink { get; set; } = string.Empty; // لینک جلسه آنلاین
    public string? MentorId { get; set; } // شناسه منتور
    public ApplicationUser? Mentor { get; set; } // ارتباط با منتور
    public string? MentorNote { get; set; } // یادداشت‌های جلسه
    public bool IsDeleted { get; set; } = false; // وضعیت حذف

    public ICollection<SessionAttendanceLog> AttendanceLogs { get; set; } = new List<SessionAttendanceLog>(); // لاگ‌های حضور

}
