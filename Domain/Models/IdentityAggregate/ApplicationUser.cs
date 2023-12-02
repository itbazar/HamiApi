using Microsoft.AspNetCore.Identity;

namespace Domain.Models.IdentityAggregate;
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public string PhoneNumber2 { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public DateTime? VerificationSent { get; set; }
    public string FcmToken { get; set; } = string.Empty;
}


