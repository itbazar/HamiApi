using Domain.Primitives;

namespace Domain.Models.DiseaseAggregate;

public class Disease : Entity
{
    // Constructor خصوصی
    private Disease(Guid id) : base(id) { }

    // متد استاتیک برای ایجاد Disease
    public static Disease Create(string name, string description)
    {
        var disease = new Disease(Guid.Empty);
        disease.Title = name;
        disease.Description = description;
        disease.IsDeleted = false;
        return disease;
    }

    // متد Update برای به‌روزرسانی
    public void Update(string? name, string? description)
    {
        Title = name ?? Title;
        Description = description ?? Description;
    }

    // متد Delete برای حذف منطقی
    public void Delete(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }

    // ویژگی‌های Disease
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
}
