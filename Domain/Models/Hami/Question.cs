using Domain.ExtensionMethods;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate.Encryption;
using Domain.Models.IdentityAggregate;
using Domain.Models.PublicKeys;
using Domain.Primitives;
using FluentResults;
using SharedKernel.Errors;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace Domain.Models.Hami;

public class Question : Entity
{
    private Question(Guid id) : base(id) { }
    private Question() : base(Guid.NewGuid()) { }
    public static Question Create(TestType testType, string questionText, Guid? id = null, bool isDeleted = false)
    {
        return new Question(id ?? Guid.NewGuid()) // مقداردهی Id از طریق سازنده
        {
            TestType = testType,
            QuestionText = questionText,
            IsDeleted = isDeleted // مقدار پیش‌فرض false است
        };
    }

    public void Update(TestType? testType, string? questionText)
    {
        TestType = testType ?? TestType;
        QuestionText = questionText ?? QuestionText;
    }

    public void Delete(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }

    public TestType TestType { get; set; } // GAD یا MDD
    public string QuestionText { get; set; } = null!;
    public bool IsDeleted { get; set; } = false;
}
#region Enums
public enum TestType
{
    GAD = 1, // Generalized Anxiety Disorder
    MDD = 2,  // Major Depressive Disorder
    MOOD = 3  // Daily Mood
}
#endregion
