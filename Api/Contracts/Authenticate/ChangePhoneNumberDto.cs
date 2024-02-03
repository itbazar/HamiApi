namespace Api.Contracts.Authenticate;

public record ChangePhoneNumberDto(
    string Token1,
    string Code1,
    string Token2,
    string Code2);