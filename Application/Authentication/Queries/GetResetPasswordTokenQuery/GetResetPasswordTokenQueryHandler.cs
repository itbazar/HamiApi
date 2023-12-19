using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Queries.GetResetPasswordTokenQuery;

internal sealed class GetResetPasswordTokenQueryHandler : IRequestHandler<GetResetPasswordTokenQuery, string>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ICaptchaProvider _captchaProvider;

    public GetResetPasswordTokenQueryHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider)
    {
        _authenticationService = authenticationService;
        _captchaProvider = captchaProvider;
    }
    public async Task<string> Handle(GetResetPasswordTokenQuery request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = _captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                throw new Exception("Invalid captcha");
            }
        }
        var resetPasswordToken = await _authenticationService.GetResetPasswordToken(request.Username, request.verificationCode);

        return resetPasswordToken;
    }
}
