namespace Api.Contracts.Authenticate;

public record GetProfileDto(
    string UserName,
    string FirstName,
    string LastName,
    string Title,
    string PhoneNumber);
