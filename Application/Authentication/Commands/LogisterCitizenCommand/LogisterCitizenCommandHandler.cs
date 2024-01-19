using Application.Common.Errors;
using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.LogisterCitizenCommand;

internal class LogisterCitizenCommandHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider, ICommunicationService communicationService) : IRequestHandler<LogisterCitizenCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LogisterCitizenCommand request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                return AuthenticationErrors.InvalidCaptcha;
            }
        }

        var result = await authenticationService.LogisterCitizen(request.PhoneNumber);
        try
        {
            await communicationService.SendVerificationAsync(
                result.PhoneNumber, result.Code);
        }
        catch
        {
            return CommunicationErrors.SmsError;
        }

        return result.Token;
    }
}
