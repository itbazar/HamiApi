using Application.Common.Exceptions;
using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.LoginCommand;

internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ICaptchaProvider _captchaProvider;
    private readonly ICommunicationService _communicationService;

    public LoginCommandHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider, ICommunicationService communicationService)
    {
        _authenticationService = authenticationService;
        _captchaProvider = captchaProvider;
        _communicationService = communicationService;
    }
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if(request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = _captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                throw new InvalidCaptchaException();
            }
        }

        var result = await _authenticationService.Login(request.Username, request.Password, true);
        if(result.AuthToken is not null)
        {
            return new LoginResponse(result.AuthToken, null);
        }
        if(result.VerificationToken is not null)
        {
            try
            {
                await _communicationService.SendVerificationAsync(
                    result.VerificationToken.PhoneNumber, result.VerificationToken.Code);
            }
            catch
            {
                throw new SendSmsException();
            }

            return new LoginResponse(null, result.VerificationToken.Token);
        }

        throw new Exception("Unpredictable behaviour.");
    }
}

