using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Security;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;

namespace Application.Authentication.Commands.LogisterCitizenCommand;

internal class LogisterCitizenCommandHandler(IAuthenticationService authenticationService,
    ICaptchaProvider captchaProvider,
    ICommunicationService communicationService,
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration) : IRequestHandler<LogisterCitizenCommand, Result<string>>
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

        //var isCaptchaValid = await VerifyRecaptcha(request.RecaptchaToken);
        //if (!isCaptchaValid)
        //{
        //    return AuthenticationErrors.InvalidCaptcha;
        //}

        //var tokenResult = await authenticationService.LogisterCitizen(request.PhoneNumber);
        return await authenticationService.PreRegisterPatient(request.PhoneNumber);
        //if (tokenResult.IsFailed)
        //    return tokenResult.ToResult();
        //var result = tokenResult.Value;
        //try
        //{
        //    await communicationService.SendVerificationAsync(
        //        result.PhoneNumber, result.Code);
        //}
        //catch
        //{
        //    return CommunicationErrors.SmsError;
        //}

        //return result;
    }

    private async Task<bool> VerifyRecaptcha(string recaptchaToken)
    {
        var secretKey = configuration["Recaptcha:SecretKey"];
        var url = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaToken}";

        var client = httpClientFactory.CreateClient();
        var response = await client.PostAsync(url, null);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var recaptchaResult = JsonSerializer.Deserialize<RecaptchaResponse>(jsonResponse);

        return recaptchaResult.success;
    }
}
public class RecaptchaResponse
{
    public bool success { get; set; }
}
