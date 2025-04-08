using Application.Common.Helper;
using Application.Common.Interfaces.Security;
using SharedKernel.Successes;

namespace Application.Authentication.Commands.ResetPasswordCommand;

internal sealed class ResetPasswordCommandHandler(
    IAuthenticationService authenticationService,
    ICaptchaProvider captchaProvider) : IRequestHandler<ResetPasswordCommand, Result<bool>>
{
    
    public async Task<Result<bool>> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                return AuthenticationErrors.InvalidCaptcha;
            }
        }
        var resetPasswordResult = await authenticationService.ResetPassword(
            request.OtpToken, request.Code, request.NewPassword);
        if (resetPasswordResult.IsFailed)
            return resetPasswordResult.ToResult();

        return ResultMethods.GetResult(resetPasswordResult.Value, UpdateSuccess.Password);
    }
}
