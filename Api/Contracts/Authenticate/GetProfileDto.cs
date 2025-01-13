namespace Api.Contracts.Authenticate;

public record GetProfileDto(
    string UserName,
    string FirstName,
    string LastName,
    string NationalId,
    string Title,
    string PhoneNumber,
    string MentorName,
    string PatientGroupName);
