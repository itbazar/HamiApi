namespace CommunicationContracts;

public class MessageBrokerNotif
{
    public string? Username { get; set; } = string.Empty;
    public List<string> ConnectionIds { get; set; } = new List<string>();
    public string MethodName {  get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;

}
