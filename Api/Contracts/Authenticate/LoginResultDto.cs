namespace Api.Contracts.Authenticate;

public record LoginResultDto(string JwtToken, string RefreshToken);