using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Infrastructure.Communications.PushNotification;

public class FirebaseCloudMessaging : IFirebaseCloudMessaging
{
    FirebaseApp _firebaseApp;
    public FirebaseCloudMessaging()
    {
        try
        {
            _firebaseApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "key.json")),
            }, "shahrbin");
        }
        catch
        {
            _firebaseApp = FirebaseApp.GetInstance("shahrbin");
        }

    }

    public async Task<string> SendNotification(FirebaseAdmin.Messaging.Message message)
    {
        var messaging = FirebaseAdmin.Messaging.FirebaseMessaging.GetMessaging(_firebaseApp);
        var result = await messaging.SendAsync(message);

        return result;
    }

    public async Task<int> SendNotification(FirebaseAdmin.Messaging.MulticastMessage message)
    {
        var messaging = FirebaseAdmin.Messaging.FirebaseMessaging.GetMessaging(_firebaseApp);
        var result = await messaging.SendMulticastAsync(message);

        if (result == null)
            return -1;

        return result.SuccessCount;
    }
}
