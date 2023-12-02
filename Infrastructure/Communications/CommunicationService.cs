using Application.Common.Interfaces.Communication;
using Domain.Models;
using Domain.Models.Common;
using Infrastructure.Communications.Sms;

namespace Infrastructure.Communications;

public class CommunicationService : ICommunicationService
{
    private readonly ISmsService _smsService;

    public CommunicationService(ISmsService smsService)
    {
        _smsService = smsService;
    }

    public async Task<int> SendAsync(string receptor, string message)
    {
        return await _smsService.SendAsync(receptor, message);
    }

    public async Task<int> SendVerificationAsync(string receptor, string message)
    {
        return await _smsService.SendVerificationAsync(receptor, message);
    }

    public void SendNotification(Message message)
    {
        throw new NotImplementedException();
    }
}
