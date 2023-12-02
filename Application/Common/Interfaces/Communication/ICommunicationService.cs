using Domain.Models.Common;

namespace Application.Common.Interfaces.Communication;

public interface ICommunicationService
{
    public void SendNotification(Message message);
    public Task<int> SendAsync(string receptor, string message);
    public Task<int> SendVerificationAsync(string receptor, string message);
}
