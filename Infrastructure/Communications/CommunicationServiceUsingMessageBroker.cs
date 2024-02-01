using Application.Common.Interfaces.Communication;
using CommunicationContracts;
using Domain.Models.Common;
using MassTransit;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace Infrastructure.Communications;

public class CommunicationServiceUsingMessageBroker : ICommunicationService
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IDatabase _database;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public CommunicationServiceUsingMessageBroker(
        IPublishEndpoint publishEndpoint,
        IConnectionMultiplexer connectionMultiplexer)
    {
        _publishEndpoint = publishEndpoint;
        _connectionMultiplexer = connectionMultiplexer;
        _database = _connectionMultiplexer.GetDatabase();
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

    public async Task AddCommunicationId(string userId, string communicationId)
    {
        var currentValue = await _database.StringGetAsync($"notif:{userId}");

        string updatedValue = currentValue.IsNull ? "" : currentValue!;
        var connectionIds = updatedValue.Split(',').ToList();
        connectionIds.Add(communicationId);
        connectionIds.RemoveAll(cid => cid.IsNullOrEmpty());
        if(connectionIds.Count > 4)
        {
            connectionIds.RemoveRange(0, connectionIds.Count - 4);
        }
        updatedValue = string.Join(",", connectionIds);

        await _database.StringSetAsync($"notif:{userId}", updatedValue);
    }

    public async Task SendNotification(string userId, string method, string message)
    {
        var currentValue = await _database.StringGetAsync($"notif:{userId}");

        string updatedValue = currentValue.IsNull ? "" : currentValue!;
        var connectionIds = updatedValue.Split(',').ToList();
        connectionIds.RemoveAll(cid => cid.IsNullOrEmpty());
        if(connectionIds.Count == 0) 
        {
            return;
        }
        await _publishEndpoint.Publish(
            new MessageBrokerNotif 
            { 
                ConnectionIds = connectionIds, 
                MethodName = method, 
                Username = "" 
            });
    }
}
