namespace CommunicationContracts;

public class MessageBrokerMessage
{
    public string Message { get; set; } = string.Empty;
    public bool IsVerification { get; set; } = false;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    /*
    public MessageBrokerMessage(
        string message,
        string phoneNumber,
        bool isVerification = false,
        string token = "",
        string topic = "",
        string condition = "")
    {
        Message = message;
        PhoneNumber = phoneNumber;
        IsVerification = isVerification;
        Token = token;
        Topic = topic;
        Condition = condition;
    }
    */
}
