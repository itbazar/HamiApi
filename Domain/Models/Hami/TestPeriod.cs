using Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models.Hami;

public class TestPeriod : Entity
{
    private TestPeriod(Guid id) : base(id) { }
    private TestPeriod() : base(Guid.NewGuid()) { }

    public static TestPeriod Create(
        TestType testType,
        string periodName,
        DateTime startDate,
        DateTime endDate,
        int code)
    {
        var testPeriod = new TestPeriod(Guid.NewGuid())
        {
            TestType = testType,
            PeriodName = periodName,
            StartDate = startDate,
            EndDate = endDate,
            Code = code
        };
        return testPeriod;
    }

    public void Update(
        TestType? testType,
        string? periodName,
        DateTime? startDate,
        DateTime? endDate,
        int? code)
    {
        TestType = testType ?? TestType;
        PeriodName = periodName ?? PeriodName;
        StartDate = startDate ?? StartDate;
        EndDate = endDate ?? EndDate;
        Code = code ?? Code;
    }

    public void Delete(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }

    public TestType TestType { get; set; } // GAD یا MDD
    public string PeriodName { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsDeleted { get; set; } = false;

    // فیلد جدید: Code
    public int Code { get; set; } // شناسه یکتا برای دسترسی سریع
}
