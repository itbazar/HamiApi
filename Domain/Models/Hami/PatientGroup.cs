using Domain.Models.IdentityAggregate;
using Domain.Primitives;

namespace Domain.Models.Hami;

public class PatientGroup : Entity
{
    private PatientGroup(Guid id) : base(id) { }

    public static PatientGroup Create(Organ organ, DiseaseType diseaseType, int? stage, string description, string? mentorId)
    {
        return new PatientGroup(Guid.NewGuid())
        {
            Organ = organ,
            DiseaseType = diseaseType,
            Stage = stage,
            Description = description,
            MentorId = mentorId
        };
    }

    public void Update(Organ? organ, DiseaseType? diseaseType, int? stage, string? description, string? mentorId)
    {
        Organ = organ ?? Organ;
        DiseaseType = diseaseType ?? DiseaseType;
        Stage = stage ?? Stage;
        Description = description ?? Description;
        MentorId = mentorId ?? MentorId;
    }

    public void Delete(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }

    public Organ Organ { get; set; } // ارگان
    public DiseaseType DiseaseType { get; set; } // نوع بیماری
    public int? Stage { get; set; } // مرحله بیماری
    public string Description { get; set; } = string.Empty; // توضیحات
    public string? MentorId { get; set; } // شناسه منتور
    public ApplicationUser? Mentor { get; set; } // ارتباط با منتور
    public bool IsDeleted { get; set; } = false; // وضعیت حذف

    public ICollection<UserGroupMembership> Members { get; set; } = new List<UserGroupMembership>(); // اعضای گروه
    public ICollection<CounselingSession> Sessions { get; set; } = new List<CounselingSession>(); // جلسات مشاوره
}
