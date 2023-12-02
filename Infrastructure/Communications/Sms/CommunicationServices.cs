namespace Api.Services.Sms;
/*
public class CommunicationServices
{
    private readonly ISmsMessaging _smsMessaging;

    public CommunicationServices(ISmsMessaging smsMessaging)
    {
        _smsMessaging = smsMessaging;
    }

    public CommunicationServices()
    {
    }

    public async Task<int> SendVerificationAsync(string receptor, string code)
    {
        return await _smsMessaging.SendVerificationAsync(receptor, code);
    }

    public async Task<int> SendReportCreatedAsync(string receptor, string trackingNumber, string password)
    {
        string message = Messaging.ReportCreated(receptor, trackingNumber, password);
        
        return await _smsMessaging.SendAsync(receptor , message);
    }

    public async Task<int> SendReportFinishedAsync(string receptor, string trackingNumber)
    {
        string message = Messaging.ReportFinished(trackingNumber);

        return await _smsMessaging.SendAsync(receptor ,message);
    }

    public async Task<int> SendContractorCreatedAsync(string receptor, string password)
    {
        string message = Messaging.ContractorCreated(receptor, password);
        
        return await _smsMessaging.SendAsync(receptor, message);
    }

    public async Task<int> SendFeedbackRequestAsync(string receptor, string url)
    {
        string message = Messaging.FeedbackRequest(url);

        return await _smsMessaging.SendAsync(receptor, message);
    }

    internal static Task AddNotification(Message message, ApplicationDbContext context)
    {
        throw new NotImplementedException();
    }

    internal static Task<int> SendNotification(Message message, ApplicationDbContext context, IFirebaseCloudMessaging messaging)
    {
        throw new NotImplementedException();
    }
*/

    /*
    public static async Task AddNotification(Message message, ApplicationDbContext context)
    {
        context.Message.Add(message);
        await context.SaveChangesAsync();
    }

    public static async Task<int> SendNotification(
        Message message,
        ApplicationDbContext context,
        IFirebaseCloudMessaging messaging)
    {
        var recipients = new List<string>();

        //Send notifications
        foreach (var recepient in message.Recepients)
        {
            switch (recepient.Type)
            {
                case RecepientType.Person:
                    recipients.Add(recepient.ToId);
                    break;
                case RecepientType.Role:
                    var ids = await context.UserRoles
                        .Where(p => p.RoleId == recepient.ToId)
                        .Select(p => p.UserId)
                        .ToListAsync();
                    recipients.AddRange(ids);
                    break;
                default:
                    break;
            }
        }

        var fcmTokens = await context.Users
            .Where(p => recipients.Contains(p.Id) && p.FcmToken != null)
            .Select(p => p.FcmToken)
            .ToListAsync();

        if (fcmTokens.Count() == 0)
        {
            return 0;
        }

        var fbMessage = new FirebaseAdmin.Messaging.MulticastMessage()
        {
            Data = new Dictionary<string, string>()
            {
                {"MessageId", message.Id.ToString() },
                {"Title", message.Title },
                {"Content", message.Content }
            },
            Notification = new FirebaseAdmin.Messaging.Notification
            {
                Title = message.Title,
                Body = message.Content,
            },
            Tokens = fcmTokens
        };

        var result = await messaging.SendNotification(fbMessage);
        return result;
    }

    */

//}
