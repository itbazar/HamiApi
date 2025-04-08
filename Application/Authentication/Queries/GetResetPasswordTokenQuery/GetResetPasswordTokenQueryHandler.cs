using Application.Common.Interfaces.Security;

namespace Application.Authentication.Queries.GetResetPasswordTokenQuery;

internal sealed class GetResetPasswordTokenQueryHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider) : IRequestHandler<GetResetPasswordTokenQuery, Result<string>>
{
    
    public async Task<Result<string>> Handle(GetResetPasswordTokenQuery request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                return AuthenticationErrors.InvalidCaptcha;
            }
        }
        var resetPasswordResult = await authenticationService.GetResetPasswordToken(
            request.Username);
        if(resetPasswordResult.IsFailed)
            return resetPasswordResult.ToResult();

        return resetPasswordResult.Value.Token;
    }
}
