namespace Infrastructure.BackgroundJobs;
/*
public class SendNotificationsHostedService : BackgroundService
{
    protected IServiceProvider _serviceProvider;

    public SendNotificationsHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }


    //
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var interval = 30;
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _context = (ApplicationDbContext)scope.ServiceProvider.GetRequiredService(typeof(ApplicationDbContext));
                    var _feedbackOptions = (IOptions<FeedbackOptions>)scope.ServiceProvider.GetRequiredService(typeof(IOptions<FeedbackOptions>));
                    var _smsOptions = (IOptions<SmsOptions>)scope.ServiceProvider.GetRequiredService(typeof(IOptions<SmsOptions>));
                    var _generalSettings = (IOptions<GeneralSettings>)scope.ServiceProvider.GetRequiredService(typeof(IOptions<GeneralSettings>));
                    interval = _generalSettings.Value.SendMessagesInterval;
                    var _communication = new CommunicationServices(_smsOptions);
                    var _messaging = (IFirebaseCloudMessaging)scope.ServiceProvider.GetRequiredService(typeof(IFirebaseCloudMessaging));

                    if (_generalSettings.Value.SendFeedbackRequests)
                    {
                        var feedbackService = new FeedbackService(_context, _feedbackOptions, _communication);
                        var result = await feedbackService.SendFeedbackRequest();
                    }

                    if (_generalSettings.Value.SendFirebasePushNotifications)
                    {
                        var messages = await getWaitingMessages(_context);
                        Message message;
                        var now = DateTime.Now;
                        for (int i = 0; i < messages.Count; i++)
                        {
                            message = messages[i];
                            if (await CommunicationServices.SendNotification(message, _context, _messaging) > 0)
                            {
                                message.LastSent = now;
                                _context.Entry(message).State = EntityState.Modified;
                            }
                        }
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {

            }

            await Task.Delay(new TimeSpan(0, 0, interval));
        }
    }

    private async Task<List<Message>> getWaitingMessages(ApplicationDbContext context)
    {
        var result = await context.Message
            .Where(p => p.LastSent == null)
            .Include(p => p.Recepients)
            .ToListAsync();

        return result;
    }

}
*/
