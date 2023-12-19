using Application.Common.Interfaces.Communication;
using CommunicationContracts;
using Domain.Models.Common;
using MassTransit;

namespace Infrastructure.Communications;

public class CommunicationServiceUsingMessageBroker : ICommunicationService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public CommunicationServiceUsingMessageBroker(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task<int> SendAsync(string receptor, string message)
    {
        await _publishEndpoint.Publish(new MessageBrokerMessage { Message = message, PhoneNumber = receptor });
        return 0;
    }

    public async Task<int> SendVerificationAsync(string receptor, string message)
    {
        await _publishEndpoint.Publish(new MessageBrokerMessage { Message = message, PhoneNumber = receptor, IsVerification = true });
        return 0;
    }

    public void SendNotification(Message message)
    {
        throw new NotImplementedException();
    }
}
