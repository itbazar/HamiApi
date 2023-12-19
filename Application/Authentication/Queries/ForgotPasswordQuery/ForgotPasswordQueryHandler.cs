using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Queries.ForgotPasswordQuery;

internal sealed class ForgotPasswordQueryHandler : IRequestHandler<ForgotPasswordQuery, bool>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ICaptchaProvider _captchaProvider;
    private readonly ICommunicationService _communicationService;

    public ForgotPasswordQueryHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider, ICommunicationService communicationService)
    {
        _authenticationService = authenticationService;
        _captchaProvider = captchaProvider;
        _communicationService = communicationService;
    }
    public async Task<bool> Handle(ForgotPasswordQuery request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = _captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                throw new Exception("Invalid captcha");
            }
        }
        var verificationCode = await _authenticationService.GetVerificationCode(request.Username);
        try
        {
            await _communicationService.SendVerificationAsync(request.Username, verificationCode);
        }
        catch
        {
            throw new Exception("There was a problem in sending sms.");
        }

        return true;
    }
}
