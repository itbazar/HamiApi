namespace Infrastructure.Communications.Sms.Panels;

public class MagfaSms : ISmsService
{
    private readonly _magfaInfo _magfaInfo;

    public MagfaSms(_magfaInfo magfaInfo)
    {
        _magfaInfo = magfaInfo;
    }

    public async Task<int> SendAsync(string receptor, string message)
    {
        var client = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = false
        });

        var url = $"{_magfaInfo.Url}?" +
            $"service=enqueue&" +
            $"username={_magfaInfo.Username}&" +
            $"password={_magfaInfo.Password}&" +
            $"domain={_magfaInfo.Domain}&" +
            $"from={_magfaInfo.PhoneNumber}&" +
            $"to={receptor}&" +
            $"text={message}";

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

        var url = $"{_magfaInfo.Url}?" +
            $"service=enqueue&" +
            $"username={_magfaInfo.Username}&" +
            $"password={_magfaInfo.Password}&" +
            $"domain={_magfaInfo.Domain}&" +
            $"from={_magfaInfo.PhoneNumber}&" +
            $"to={receptor}&" +
            $"text={message}";

        await client.GetAsync(url);

        return 0;
    }
}
public record _magfaInfo(string PhoneNumber, string Url, string Username, string Password, string Domain);