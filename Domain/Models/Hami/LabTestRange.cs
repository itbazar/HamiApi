using Domain.Models.Hami;
using Domain.Primitives;

public class LabTestRange : Entity
{
    private LabTestRange(Guid id) : base(id) { }
    private LabTestRange() : base(Guid.NewGuid()) { }

    public static LabTestRange Create(
        TestType testType,
        string category, // مثلاً "40-49 سال" یا "سیگاری"
        decimal minValue,
        decimal maxValue)
    {
        return new LabTestRange(Guid.NewGuid())
        {
            TestType = testType,
            Category = category,
            MinValue = minValue,
            MaxValue = maxValue
        };
    }

    public TestType TestType { get; set; }  // نوع آزمایش (PSA, CEA, ...)
    public string Category { get; set; } = null!; // دسته‌بندی (سن، سیگاری بودن و ...)
    public decimal MinValue { get; set; }
    public decimal MaxValue { get; set; }
}
