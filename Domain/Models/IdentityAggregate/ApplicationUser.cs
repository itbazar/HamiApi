using Domain.Models.Hami;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Domain.Models.IdentityAggregate;
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public string PhoneNumber2 { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public DateTime? VerificationSent { get; set; }
    public string FcmToken { get; set; } = string.Empty;


    public DateTime DateOfBirth { get; set; } // تاریخ تولد
    public Gender Gender { get; set; } // جنسیت (Male, Female, Other)
    public RoleType RoleType { get; set; } 
    public EducationLevel? Education { get; set; } // تحصیلات
    public string? City { get; set; } // شهر محل سکونت
    public bool IsSmoker { get; set; }
    

    public RegistrationStatus RegistrationStatus { get; set; } = RegistrationStatus.Pending; // وضعیت ثبت‌نام
    public string? RejectionReason { get; set; } // دلیل رد ثبت‌نام (در صورت وجود)

    [JsonIgnore]
    public ICollection<UserGroupMembership> UserGroupMemberships { get; set; } = new List<UserGroupMembership>(); // اعضای گروه


}

public enum Gender
{
    Male = 1,   // مرد
    Female = 2, // زن
    Other = 3   // سایر
}

public enum EducationLevel
{
    None = 0,           // بدون تحصیلات
    HighSchool = 1,     // دیپلم
    Bachelor = 2,       // لیسانس
    Master = 3,         // فوق لیسانس
    Doctorate = 4,      // دکتری
    Other = 5           // سایر
}

// وضعیت ثبت‌نام
public enum RegistrationStatus
{
    Pending = 1,    // منتظر تأیید
    Approved = 2,   // تأییدشده
    Rejected = 3    // ردشده
}

public enum RoleType
{
    Patient = 1,   // بیمار
    Caregiver = 2, // مراقب بیمار
}