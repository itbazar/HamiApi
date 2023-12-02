namespace Infrastructure.Communications.Sms.Panels;

public class YekBYekSms : ISmsService
{
    public YekBYekSms()
    {

    }

    public async Task<int> SendAsync(string receptor, string message)
    {
        //TODO: Should be implemented
        await Task.CompletedTask;
        return 0;
        /*
        YekBYekService.SMSwsdlPortTypeClient sMSwsdlPortTypeClient = new YekBYekService.SMSwsdlPortTypeClient();
        await sMSwsdlPortTypeClient.sendSMSAsync(
            _smsOptions.YekBYekDomain, _smsOptions.YekBYekUsername,
            _smsOptions.YekBYekPassword, _smsOptions.YekBYekPhoneNumber,
            receptor, message, 1
            );

        return 0;
        */
    }

    public async Task<int> SendVerificationAsync(string receptor, string message)
    {
        //TODO: Should be implemented
        await Task.CompletedTask;
        return 0;
        /*
        YekBYekService.SMSwsdlPortTypeClient sMSwsdlPortTypeClient = new YekBYekService.SMSwsdlPortTypeClient();
        await sMSwsdlPortTypeClient.sendSMSAsync(
            _smsOptions.YekBYekDomain, _smsOptions.YekBYekUsername,
            _smsOptions.YekBYekPassword, _smsOptions.YekBYekPhoneNumber,
            receptor, message, 1
            );

        return 0;
        */
    }
}
