using Domain.Models.IdentityAggregate;

namespace Application.Users.Common;

public record PatientResponse(string Id, string FirstName, string LastName, string UserName, DateTime DateOfBirth, string GroupName, string City, RegistrationStatus RegistrationStatus);
public record MentorResponse(string Id, string FirstName, string LastName, string UserName, string Email, string PhoneNumber, string City, Gender Gender,EducationLevel? EducationLevel);