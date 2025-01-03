using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami.Events;
using SharedKernel.Statics;

namespace Application.Users.Events;

internal sealed class PatientCreatedDomainEventHandler(
    IUserRepository userRepository,
    ICommunicationService communicationService) : INotificationHandler<PatientCreatedDomainEvent>
{
    public async Task Handle(PatientCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.UserId is null)
            return;
       
        string message = $"کاربری شما با شماره همراه  {notification.PhoneNumber} در سامانه ثبت شد.";
        string message2 = $"  پس از بررسی و تایید کاربری به شما اطلاع رسانی خواهد شد";
        await communicationService.SendAsync(notification.PhoneNumber, message+message2);
    }
}
