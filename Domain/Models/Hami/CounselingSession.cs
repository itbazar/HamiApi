using Domain.Models.Hami;
using Domain.Models.Hami.Events;
using Domain.Models.IdentityAggregate;
using Domain.Primitives;
using System.Text.Json.Serialization;

public class CounselingSession : Entity
{
    private CounselingSession(Guid id) : base(id) { }

    public static CounselingSession Create(Guid patientGroupId, string mentorId, DateTime scheduledDate, string topic, string meetingLink, string? mentorNote = null, bool isConfirmed = false)
    {
        CounselingSession info = new(Guid.NewGuid());
        info.PatientGroupId = patientGroupId;
        info.MentorId = mentorId;
        info.ScheduledDate = scheduledDate;
        info.Topic = topic;
        info.MeetingLink = meetingLink;
        info.MentorNote = mentorNote;
        info.IsConfirmed = isConfirmed;
        info.Raise(new AddCounselingSessionDomainEvent(
            patientGroupId,
            mentorId,
            scheduledDate,
            topic,
            meetingLink));
        return info;
    }

    public void Update(DateTime? scheduledDate, string? topic, string? meetingLink, string? mentorNote, bool? isConfirmed = null)
    {
        ScheduledDate = scheduledDate ?? ScheduledDate;
        Topic = topic ?? Topic;
        MeetingLink = meetingLink ?? MeetingLink;
        MentorNote = mentorNote ?? MentorNote;
        IsConfirmed = isConfirmed ?? IsConfirmed;
    }

    public void Delete(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }

    public Guid PatientGroupId { get; set; } // کلید خارجی به گروه بیماران
    [JsonIgnore]
    public PatientGroup PatientGroup { get; set; } = null!; // ارتباط با گروه
    public DateTime ScheduledDate { get; set; } // تاریخ و زمان جلسه
    public string Topic { get; set; } = string.Empty; // موضوع جلسه مشاوره
    public string MeetingLink { get; set; } = string.Empty; // لینک جلسه آنلاین
    public string? MentorId { get; set; } // شناسه منتور
    [JsonIgnore]
    public ApplicationUser? Mentor { get; set; } // ارتباط با منتور
    public string? MentorNote { get; set; } // یادداشت‌های جلسه
    public bool IsConfirmed { get; set; } = false; // تایید برگزاری جلسه
    public bool IsDeleted { get; set; } = false; // وضعیت حذف

    public ICollection<SessionAttendanceLog> AttendanceLogs { get; set; } = new List<SessionAttendanceLog>(); // لاگ‌های حضور
}
