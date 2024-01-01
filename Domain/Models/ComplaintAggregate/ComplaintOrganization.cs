using Domain.Primitives;

namespace Domain.Models.ComplaintAggregate;

public class ComplaintOrganization : Entity
{
    private ComplaintOrganization(Guid id) : base(id) { }
    public static ComplaintOrganization Create(string title, string description)
    {
        var organization = new ComplaintOrganization(Guid.Empty);
        organization.Title = title;
        organization.Description = description;
        return organization;
    }

    public void Update(string? title, string? description)
    {
        Title = title ?? Title;
        Description = description ?? Description;
    }

    public void Delete(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
}
