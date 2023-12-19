using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.ChangePasswordCommand;

internal sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ICaptchaProvider _captchaProvider;

    public ChangePasswordCommandHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider)
    {
        _authenticationService = authenticationService;
        _captchaProvider = captchaProvider;
    }
    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = _captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                throw new Exception("Invalid captcha");
            }
        }
        return await _authenticationService.ChangePassword(request.Username, request.OldPassword, request.NewPassword);
    }
}
