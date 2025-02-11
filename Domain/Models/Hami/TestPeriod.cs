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
        int code,
        RecurrenceType recurrence)
    {
        var testPeriod = new TestPeriod(Guid.NewGuid())
        {
            TestType = testType,
            PeriodName = periodName,
            StartDate = startDate,
            EndDate = endDate,
            Code = code,
            Recurrence = recurrence,
            NextOccurrence = recurrence != RecurrenceType.None ? startDate : null // اگر تکرار ندارد، مقدار null
        };
        return testPeriod;
    }

    public void Update(
        TestType? testType,
        string? periodName,
        DateTime? startDate,
        DateTime? endDate,
        int? code,
        RecurrenceType? recurrence)
    {
        TestType = testType ?? TestType;
        PeriodName = periodName ?? PeriodName;
        StartDate = startDate ?? StartDate;
        EndDate = endDate ?? EndDate;
        Code = code ?? Code;
        Recurrence = recurrence ?? Recurrence;
    }

    public void Delete(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }

    public void UpdateNextOccurrence()
    {
        if (Recurrence == RecurrenceType.None || NextOccurrence == null)
            return;

        var newNextOccurrence = Recurrence switch
        {
            RecurrenceType.Daily => NextOccurrence.Value.AddDays(1),
            RecurrenceType.Weekly => NextOccurrence.Value.AddDays(7),
            RecurrenceType.Monthly => NextOccurrence.Value.AddMonths(1),
            _ => NextOccurrence
        };

        // اگر تاریخ جدید از EndDate عبور کند
        if (newNextOccurrence > EndDate)
        {
            NextOccurrence = null; // دوره به پایان رسیده است
        }
        else
        {
            NextOccurrence = newNextOccurrence;
        }
    }


    public TestType TestType { get; set; } // GAD یا MDD
    public string PeriodName { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public RecurrenceType Recurrence { get; set; } = RecurrenceType.None; // نوع تکرار
    public DateTime? NextOccurrence { get; set; } // زمان تکرار بعدی
    public int Code { get; set; } // شناسه یکتا برای دسترسی سریع
    public bool IsDeleted { get; set; } = false;
}

public enum RecurrenceType
{
    None=0,
    Daily=1,
    Weekly=2,
    Monthly=3
}
