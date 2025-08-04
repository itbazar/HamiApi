using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami.Events;
using SharedKernel.Statics;

namespace Application.Users.Events;

internal sealed class PatientApprovedDomainEventHandler(
    IUserRepository userRepository,
    ICommunicationService communicationService) : INotificationHandler<PatientApprovedDomainEvent>
{
    public async Task Handle(PatientApprovedDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.UserId is null)
            return;
       
        string message = $"کاربری شما با شماره همراه  {notification.PhoneNumber} در سامانه توسط اپراتور تایید شد.";
        string message2 = $"  اکنون میتوانید با نام کاربری {notification.PhoneNumber} و رمز عبور خود وارد سامانه شوید";
        
        await communicationService.SendAsync(notification.PhoneNumber, message+message2);
        //await communicationService.SendNotification(
        //    notification.UserId,
        //    "Created",
        //    message,
        //    notification.ComplaintId);
    }
}


