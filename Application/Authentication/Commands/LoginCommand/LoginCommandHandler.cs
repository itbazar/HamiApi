﻿using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Security;

namespace Application.Authentication.Commands.LoginCommand;

internal sealed class LoginCommandHandler(
    IAuthenticationService authenticationService,
    ICaptchaProvider captchaProvider,
    ICommunicationService communicationService) : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if(request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                return AuthenticationErrors.InvalidCaptcha;
            }
        }

        var loginResult = await authenticationService.Login(request.Username, request.Password, true);
        if (loginResult.IsFailed)
            return loginResult.ToResult();
        
        var result = loginResult.Value;

        if(result.VerificationToken is not null)
        {
            try
            {
                await communicationService.SendVerificationAsync(
                    result.VerificationToken.PhoneNumber, result.VerificationToken.Code);
            }
            catch
            {
                return CommunicationErrors.SmsError;
            }

            return result.VerificationToken.Token;
        }

        throw new Exception("Unpredictable behaviour.");
    }
}

