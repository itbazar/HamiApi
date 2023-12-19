using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.VerifyPhoneNumberCommand;

internal sealed class VerifyPhoneNumberCommandHandler : IRequestHandler<VerifyPhoneNumberCommand, bool>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ICaptchaProvider _captchaProvider;

    public VerifyPhoneNumberCommandHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider)
    {
        _authenticationService = authenticationService;
        _captchaProvider = captchaProvider;
    }
    public async Task<bool> Handle(VerifyPhoneNumberCommand request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = _captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                throw new Exception("Invalid captcha");
            }
        }
        var isVerified = await _authenticationService.VerifyPhoneNumber(request.Username, request.verificationCode);

        return isVerified;
    }
}
