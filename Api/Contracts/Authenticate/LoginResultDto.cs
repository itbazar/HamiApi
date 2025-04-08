namespace Api.Contracts.Authenticate;

public record LoginResultDto(string JwtToken, string RefreshToken); 
public record ForgetPasswordResultDto(string PhoneNumber,string Token, string Code);