using Application.Common.Exceptions;
using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.LogisterCitizenCommand;

internal class LogisterCitizenCommandHandler : IRequestHandler<LogisterCitizenCommand, string>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ICaptchaProvider _captchaProvider;
    private readonly ICommunicationService _communicationService;

    public LogisterCitizenCommandHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider, ICommunicationService communicationService)
    {
        _authenticationService = authenticationService;
        _captchaProvider = captchaProvider;
        _communicationService = communicationService;
    }
    public async Task<string> Handle(LogisterCitizenCommand request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = _captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                throw new InvalidCaptchaException();
            }
        }

        var result = await _authenticationService.LogisterCitizen(request.PhoneNumber);
        try
        {
            await _communicationService.SendVerificationAsync(
                result.PhoneNumber, result.Code);
        }
        catch
        {
            throw new SendSmsException();
        }

        return result.Token;
    }
}
