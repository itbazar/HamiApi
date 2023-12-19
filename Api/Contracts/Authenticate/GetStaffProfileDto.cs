namespace Api.Contracts.Authenticate;

public record GetStaffProfileDto(
    string UserName,
    string FirstName,
    string LastName,
    string Title,
    string PhoneNumber);
