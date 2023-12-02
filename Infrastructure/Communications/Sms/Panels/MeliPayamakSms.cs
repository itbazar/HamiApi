namespace Infrastructure.Communications.Sms.Panels;

public class MeliPayamakSms : ISmsService
{
    private readonly MeliPayamakInfo _meliPayamakInfo;

    public MeliPayamakSms(MeliPayamakInfo meliPayamakInfo)
    {
        _meliPayamakInfo = meliPayamakInfo;
    }

    public async Task<int> SendAsync(string receptor, string message)
    {
        mpNuget.RestClient restClient = new mpNuget.RestClient(_meliPayamakInfo.Username, _meliPayamakInfo.Password);
        restClient.Send(receptor, _meliPayamakInfo.PhoneNumber, message, true);

        await Task.CompletedTask;
        return 0;
    }

    public async Task<int> SendVerificationAsync(string receptor, string message)
    {
        mpNuget.RestClient restClient = new mpNuget.RestClient(_meliPayamakInfo.Username, _meliPayamakInfo.Password);
        restClient.SendByBaseNumber(message, receptor, int.Parse(_meliPayamakInfo.PatternId));

        await Task.CompletedTask;
        return 0;
    }
}

public record MeliPayamakInfo(string PhoneNumber, string Username, string Password, string PatternId);