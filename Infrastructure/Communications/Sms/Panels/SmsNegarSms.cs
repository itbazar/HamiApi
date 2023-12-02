namespace Infrastructure.Communications.Sms.Panels;

public class SmsNegarSms : ISmsService
{
    private readonly SmsNegarInfo _smsNegarInfo;

    public SmsNegarSms(SmsNegarInfo smsNegarInfo)
    {
        _smsNegarInfo = smsNegarInfo;
    }

    public async Task<int> SendAsync(string receptor, string message)
    {
        var client = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = false
        });

        var url = $"{_smsNegarInfo.Url}?" +
            $"cbody={message}&cmobileno={receptor}&" +
            $"cUsername={_smsNegarInfo.Username}&cpassword={_smsNegarInfo.Password}&" +
            $"cDomainName={_smsNegarInfo.Domain}&cEncoding=1&cfromnumber={_smsNegarInfo.PhoneNumber}";

        await client.GetAsync(url);

        return 0;
    }

    public async Task<int> SendVerificationAsync(string receptor, string message)
    {
        message = "سامانه شهربین\r\nکدورود:\r\n" + message;
        var client = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = false
        });

        var url = $"{_smsNegarInfo.Url}?" +
            $"cbody={message}&cmobileno={receptor}&" +
            $"cUsername={_smsNegarInfo.Username}&cpassword={_smsNegarInfo.Password}&" +
            $"cDomainName={_smsNegarInfo.Domain}&cEncoding=1&cfromnumber={_smsNegarInfo.PhoneNumber}";

        await client.GetAsync(url);

        return 0;
    }
}
public record SmsNegarInfo(string PhoneNumber, string Url, string Username, string Password, string Domain);