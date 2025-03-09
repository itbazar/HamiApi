using Domain.Models.IdentityAggregate;
using Domain.Primitives;

namespace Domain.Models.Hami;

public class PatientLabTest : Entity
{
    private PatientLabTest(Guid id) : base(id) { }
    private PatientLabTest() : base(Guid.NewGuid()) { }

    public static PatientLabTest Create(
        string userId,
        LabTestType testType,
        decimal testValue,
        string unit)
    {
        var labTest = new PatientLabTest(Guid.NewGuid())
        {
            UserId = userId,
            TestType = testType,
            TestValue = testValue,
            Unit = unit,
            CreatedAt = DateTime.UtcNow
        };
        return labTest;
    }

    public void Update(decimal testValue)
    {
        TestValue = testValue;
    }

    public void Delete(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }

    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public LabTestType TestType { get; set; } // نوع آزمایش
    public decimal TestValue { get; set; } // مقدار آزمایش
    public string Unit { get; set; } = null!; // واحد آزمایش (مثلاً ng/ml)
    public DateTime TestDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
}

public enum LabTestType
{
    PSA = 1,    // Prostate-Specific Antigen
    CEA = 2,    // Carcinoembryonic Antigen
    CA125 = 3,  // Cancer Antigen 125
    CA15_3 = 4, // Cancer Antigen 15-3
    CA27_29 = 5 // Cancer Antigen 27-29
}

public enum LabTestCategory
{
    // PSA بر اساس سن
    PSA_40_49,   // 40-49 سال
    PSA_50_59,   // 50-59 سال
    PSA_60_69,   // 60-69 سال
    PSA_70_79,   // 70-79 سال

    // CEA بر اساس وضعیت سیگاری بودن
    CEA_NonSmoker,  // افراد غیرسیگاری
    CEA_Smoker,     // افراد سیگاری

    // سایر آزمایش‌ها بدون دسته‌بندی خاص
    CA125,   // CA-125
    CA15_3,  // CA-15-3
    CA27_29  // CA-27-29
}
