namespace Api.Contracts.Authenticate;

public record RefreshDto(
    string Token,
    string RefreshToken);