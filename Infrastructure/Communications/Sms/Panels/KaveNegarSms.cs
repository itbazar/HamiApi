namespace Infrastructure.Communications.Sms.Panels;

public class KaveNegarSms : ISmsService
{
    private readonly KaveNegarInfo _kavehNegarInfo;

    public KaveNegarSms(KaveNegarInfo kavehNegarInfo)
    {
        _kavehNegarInfo = kavehNegarInfo;
    }

    public async Task<int> SendAsync(string receptor, string message)
    {
        string apiKey = _kavehNegarInfo.ApiKey;
        string phoneNumber = _kavehNegarInfo.PhoneNumber;
        Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);
        await api.Send(phoneNumber, receptor, message);

        return 0;
    }

    public async Task<int> SendVerificationAsync(string receptor, string message)
    {
        string apiKey = _kavehNegarInfo.ApiKey;
        Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);
        await api.VerifyLookup(receptor, message, _kavehNegarInfo.TemplateName);

        return 0;
    }
}

public record KaveNegarInfo(string PhoneNumber, string ApiKey, string TemplateName);