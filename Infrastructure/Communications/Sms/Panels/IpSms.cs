namespace Infrastructure.Communications.Sms.Panels;

public class IpSms : ISmsService
{
    private readonly IpPanelInfo _ipPanelInfo;

    public IpSms(IpPanelInfo ipPanelInfo)
    {
        _ipPanelInfo = ipPanelInfo;
    }

    public async Task<int> SendAsync(string receptor, string message)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("AccessKey", _ipPanelInfo.AccessKey);
        var smsApi = new IpPanelSmsApi.SmsApi(client);
        var response = await smsApi.SendSMSAsync(
            new IpPanelSmsApi.Body()
            {
                Message = message,
                Originator = _ipPanelInfo.PhoneNumber,
                Recipients = new List<string>() { receptor }
            });
        //TODO: Consider return values
        return 0;
    }

    public async Task<int> SendVerificationAsync(string receptor, string message)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("AccessKey", _ipPanelInfo.AccessKey);
        var smsApi = new IpPanelSmsApi.SmsApi(client);
        var response = await smsApi.SendPatternAsync(new IpPanelSmsApi.Body3()
        {
            Originator = _ipPanelInfo.PhoneNumber,
            Pattern_code = _ipPanelInfo.PatternId,
            Recipient = receptor,
            Values = new { Code = message }
        });
        //TODO: Consider return values
        return 0;
    }
}

public record IpPanelInfo(string PhoneNumber, string AccessKey, string PatternId);