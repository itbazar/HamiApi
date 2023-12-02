using Domain.Primitives;

namespace Domain.Models.ComplaintAggregate;

public class ComplaintCategory : Entity
{
    private ComplaintCategory(Guid id) : base(id) { }
    public static ComplaintCategory Create(string title, string description)
    {
        var complaintCategory = new ComplaintCategory(Guid.NewGuid());
        complaintCategory.Title = title;
        complaintCategory.Description = description;
        return complaintCategory;
    }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
}
