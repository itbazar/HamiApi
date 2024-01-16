using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.LoginCommand;

public sealed record LoginCommand(
    string Username,
    string Password,
    CaptchaValidateModel? CaptchaValidateModel = null) :IRequest<LoginResponse>;
public record LoginResponse(AuthToken? AuthToken, string? VerificationToken);