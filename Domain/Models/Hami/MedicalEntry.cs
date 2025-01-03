using Domain.Primitives;
using Domain.Models.IdentityAggregate;

namespace Domain.Models.Hami;

public class MedicalEntry : Entity
{
    private MedicalEntry(Guid id) : base(id) { }

    public static MedicalEntry Create(
        Guid userMedicalInfoId,
        string createdById,
        MedicalEntryType entryType,
        string description,
        string? attachedFilePath = null)
    {
        return new MedicalEntry(Guid.NewGuid())
        {
            UserMedicalInfoId = userMedicalInfoId,
            CreatedById = createdById,
            EntryType = entryType,
            Description = description,
            AttachedFilePath = attachedFilePath
        };
    }

    // متد بروزرسانی
    public void Update(string? description, string? attachedFilePath)
    {
        Description = description ?? Description;
        AttachedFilePath = attachedFilePath ?? AttachedFilePath;
    }

    public Guid UserMedicalInfoId { get; set; } // شناسه اطلاعات پزشکی کاربر
    public UserMedicalInfo UserMedicalInfo { get; set; } = null!; // ارتباط با UserMedicalInfo

    public string CreatedById { get; set; } = string.Empty; // شناسه ثبت‌کننده
    public ApplicationUser CreatedBy { get; set; } = null!; // ارتباط با ثبت‌کننده

    public MedicalEntryType EntryType { get; set; } // نوع ورودی (یادداشت، آزمایش، و غیره)
    public string Description { get; set; } = string.Empty; // توضیحات
    public string? AttachedFilePath { get; set; } // مسیر فایل پیوست
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // تاریخ ثبت
}

public enum MedicalEntryType
{
    Note = 1,          // یادداشت
    LabTest = 2,       // آزمایش
    Imaging = 3,       // تصویربرداری
    TreatmentPlan = 4, // برنامه درمانی
    Prescription = 5,  // نسخه دارویی
    Other = 6          // سایر موارد
}