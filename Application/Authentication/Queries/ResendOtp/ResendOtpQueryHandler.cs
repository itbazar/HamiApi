using Application.Common.Helper;
using Application.Common.Interfaces.Security;
using SharedKernel.Successes;

namespace Application.Authentication.Queries.ResendOtp;

internal sealed class ResendOtpQueryHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider)
    : IRequestHandler<ResendOtpQuery, Result<string>>
{

    public async Task<Result<string>> Handle(ResendOtpQuery request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                return AuthenticationErrors.InvalidCaptcha;
            }
        }
        var resentOtpResult = await authenticationService.ResendVerificationCode(
            request.OtpToken);
        if (resentOtpResult.IsFailed)
            return resentOtpResult.ToResult();

        return ResultMethods.GetResult(resentOtpResult.Value.Token, OperationSuccess.ResendOtp);
    }
}
