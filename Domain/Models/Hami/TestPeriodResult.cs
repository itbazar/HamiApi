using Domain.Models.IdentityAggregate;
using Domain.Primitives;

namespace Domain.Models.Hami;

public class TestPeriodResult : Entity
{
    private TestPeriodResult(Guid id) : base(id) { }

    public static TestPeriodResult Create(
        string userId,
        TestType testType,
        int totalScore,
        Guid testPeriodId,
        int testInstance)
    {
        var result = new TestPeriodResult(Guid.NewGuid())
        {
            UserId = userId,
            TestType = testType,
            TotalScore = totalScore,
            TestPeriodId = testPeriodId,
            TestInstance = testInstance
        };
        return result;
    }

    public void Update(int? totalScore)
    {
        TotalScore = totalScore ?? TotalScore;
    }

    public void Delete(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }

    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; } = null;
    public TestType TestType { get; set; } // GAD یا MDD
    public int TotalScore { get; set; }
    public Guid TestPeriodId { get; set; }
    public TestPeriod TestPeriod { get; set; } = null!;
    public int TestInstance { get; set; } // شماره دوره آزمون
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // تاریخ ثبت نتیجه
    public bool IsDeleted { get; set; } = false;
}
