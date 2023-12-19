using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.ResetPasswordCommand;

internal sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ICaptchaProvider _captchaProvider;

    public ResetPasswordCommandHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider)
    {
        _authenticationService = authenticationService;
        _captchaProvider = captchaProvider;
    }
    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = _captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                throw new Exception("Invalid captcha");
            }
        }
        var isSucceeded = await _authenticationService.ResetPassword(request.Username, request.ResetPasswordToken, request.NewPassword);

        return isSucceeded;
    }
}
