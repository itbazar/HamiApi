namespace Infrastructure.Communications.PushNotification;

public interface IFirebaseCloudMessaging
{
    public Task<string> SendNotification(FirebaseAdmin.Messaging.Message message);
    public Task<int> SendNotification(FirebaseAdmin.Messaging.MulticastMessage message);
}
