﻿using Domain.Primitives;

namespace Domain.Models.ComplaintAggregate;

public class ComplaintCategory : Entity
{
    private ComplaintCategory(Guid id) : base(id) { }
    public static ComplaintCategory Create(string title, string description)
    {
        var complaintCategory = new ComplaintCategory(Guid.Empty);
        complaintCategory.Title = title;
        complaintCategory.Description = description;
        return complaintCategory;
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

    public string Title { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
}

