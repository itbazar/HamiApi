using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.LoginCommand;

public sealed record LoginCommand(string Username, string Password, CaptchaValidateModel? CaptchaValidateModel = null, string? VerificationCode = null):IRequest<LoginResultModel>;
