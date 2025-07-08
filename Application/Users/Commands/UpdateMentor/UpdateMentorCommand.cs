using Domain.Models.IdentityAggregate;

namespace Application.Users.Commands.UpdateMentor;

public record UpdateMentorCommand(
    string MentorId,
    string? FirstName = null,
    string? LastName = null,
    string? Title = null,
    string? Email = null,
    //DateTime? DateOfBirth = null,
    Gender? Gender = null,
    EducationLevel? Education = null,
    string? City = null
    ) : IRequest<Result<ApplicationUser>>;
