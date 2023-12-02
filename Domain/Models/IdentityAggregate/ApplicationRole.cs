using Microsoft.AspNetCore.Identity;

namespace Domain.Models.IdentityAggregate;

public class ApplicationRole : IdentityRole
{
    public string Title { get; set; } = string.Empty;
}


