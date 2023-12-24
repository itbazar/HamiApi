using Application.Common.Exceptions;
using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.LogisterCitizenCommand;

internal class LogisterCitizenCommandHandler : IRequestHandler<LogisterCitizenCommand, LoginResultModel>
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
    public async Task<LoginResultModel> Handle(LogisterCitizenCommand request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = _captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                throw new InvalidCaptchaException();
            }
        }

        LoginResultModel? result;
        try
        {
            result = await _authenticationService.LogisterCitizen(request.PhoneNumber, request.VerificationCode);
        }
        catch (PhoneNumberNotConfirmedException)
        {
            var verificationCode = await _authenticationService.GetVerificationCode(request.PhoneNumber);
            try
            {
                await _communicationService.SendVerificationAsync(verificationCode.PhoneNumber, verificationCode.Code);
            }
            catch
            {
                throw new SendSmsException();
            }
            result = new LoginResultModel("", true);
        }
        return result;
    }
}
