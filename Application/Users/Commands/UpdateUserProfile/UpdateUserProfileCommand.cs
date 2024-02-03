using Domain.Models.IdentityAggregate;

namespace Application.Users.Commands.UpdateUserProfile;

public record UpdateUserProfileCommand(
    string UserId,
    string? FirstName = null,
    string? LastName = null,
    string? Title = null,
    string? NationalId = null,
    string? PhoneNumber2 = null
    ) : IRequest<Result<ApplicationUser>>;
