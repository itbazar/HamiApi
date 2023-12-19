using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.ResetPasswordCommand;

public sealed record ResetPasswordCommand(string Username, string ResetPasswordToken, string NewPassword, CaptchaValidateModel? CaptchaValidateModel = null) : IRequest<bool>;
