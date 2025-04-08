using Application.Common.Interfaces.Security;

namespace Application.Authentication.Commands.ResetPasswordCommand;

public sealed record ResetPasswordCommand(
    string OtpToken,
    string Code,
    string NewPassword,
    CaptchaValidateModel? CaptchaValidateModel = null) : IRequest<Result<bool>>;
