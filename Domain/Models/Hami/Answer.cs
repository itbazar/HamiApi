using Domain.Models.IdentityAggregate;
using Domain.Primitives;

namespace Domain.Models.Hami;

public class Answer : Entity
{
    private Answer(Guid id) : base(id) { }

    private Answer() : base(Guid.NewGuid()) { }

    public static Answer Create(string userId, Guid questionId, int answerValue, Guid testPeriodId)
    {
        return new Answer(Guid.NewGuid())
        {
            UserId = userId,
            QuestionId = questionId,
            AnswerValue = answerValue,
            TestPeriodId = testPeriodId
        };
    }
    

    public void Update(int? value)
    {
        AnswerValue = value ?? AnswerValue;
    }

    public void Delete(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }

    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; } = null;
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public int AnswerValue { get; set; } // مقدار پاسخ (0 تا 3)
    public Guid TestPeriodId { get; set; }
    public TestPeriod TestPeriod { get; set; } = null!;
    public DateTime AnswerDate { get; set; } = DateTime.UtcNow; // تاریخ ثبت پاسخ
    public bool IsDeleted { get; set; } = false; // وضعیت حذف
}
