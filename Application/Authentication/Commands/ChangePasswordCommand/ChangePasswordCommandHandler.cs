using Application.Common.Interfaces.Security;

namespace Application.Authentication.Commands.ChangePasswordCommand;

internal sealed class ChangePasswordCommandHandler(
    IAuthenticationService authenticationService,
    ICaptchaProvider captchaProvider) : IRequestHandler<ChangePasswordCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                return AuthenticationErrors.InvalidCaptcha;
            }
        }
        return await authenticationService.ChangePassword(request.Username, request.OldPassword, request.NewPassword);
    }
}
