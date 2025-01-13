using Domain.Models.IdentityAggregate;

namespace Application.Users.Common;

public record PatientResponse(string Id, string FirstName, string LastName, string UserName, DateTime DateOfBirth, string GroupName, string City, RegistrationStatus RegistrationStatus);