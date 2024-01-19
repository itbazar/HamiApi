using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.ChangePasswordCommand;

public sealed record ChangePasswordCommand(
    string Username,
    string OldPassword,
    string NewPassword,
    CaptchaValidateModel? CaptchaValidateModel = null) : IRequest<Result<bool>>;
