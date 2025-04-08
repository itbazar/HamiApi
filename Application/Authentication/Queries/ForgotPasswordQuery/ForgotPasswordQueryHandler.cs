using Application.Common.Interfaces.Security;

namespace Application.Authentication.Queries.ForgotPasswordQuery;

internal sealed class ForgotPasswordQueryHandler(
    IAuthenticationService authenticationService,
    ICaptchaProvider captchaProvider) : IRequestHandler<ForgotPasswordQuery, Result<VerificationToken>>
{
    
    public async Task<Result<VerificationToken>> Handle(ForgotPasswordQuery request, CancellationToken cancellationToken)
    {
        
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
                return AuthenticationErrors.InvalidCaptcha;
        }

        var result = await authenticationService.GetResetPasswordToken(request.Username);
        if (result.IsFailed)
            return result.ToResult();

        return result;
    }
}
