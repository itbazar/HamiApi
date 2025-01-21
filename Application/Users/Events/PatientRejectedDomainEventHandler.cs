using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami.Events;
using SharedKernel.Statics;

namespace Application.Users.Events;

internal sealed class PatientRejectedDomainEventHandler(
    IUserRepository userRepository,
    ICommunicationService communicationService) : INotificationHandler<PatientRejectedDomainEvent>
{
    public async Task Handle(PatientRejectedDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.UserId is null)
            return;

        string message = $"کاربری شما با شماره همراه  {notification.PhoneNumber} در سامانه توسط اپراتور رد شد.";
        string message2 = $" دلیل ذکر شده : {notification.RejectResion} ";

        await communicationService.SendAsync(notification.PhoneNumber, message + message2);
        //await communicationService.SendNotification(
        //    notification.UserId,
        //    "Created",
        //    message,
        //    notification.ComplaintId);
    }
}


