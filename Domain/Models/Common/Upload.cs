using Domain.Models.IdentityAggregate;

namespace Domain.Models.Common;

public class Upload
{
    public Guid Id { get; set; }
    public Media Media { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool IsUsed { get; set; }
}
