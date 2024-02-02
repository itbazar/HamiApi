using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate.Events;
using SharedKernel.Statics;

namespace Application.Complaints.Events;

internal sealed class ComplaintCreatedDomainEventHandler(
    IUserRepository userRepository,
    ICommunicationService communicationService) : INotificationHandler<ComplaintCreatedDomainEvent>
{
    public async Task Handle(ComplaintCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Inspector
        string message = $"درخواست جدید با کد رهگیری {notification.TrackingNumber} در سامانه ثبت شد.";
        var inspectors = await userRepository.GetUsersInRole(RoleNames.Inspector);
        foreach (var inspector in inspectors)
        {
            if (inspector?.PhoneNumber is null)
                continue;
            await communicationService.SendAsync(inspector.PhoneNumber, message);
            await communicationService.SendNotification(
                inspector.Id,
                "Created",
                message,
                notification.ComplaintId);
        }

        // Citizen
        if (notification.UserId is null)
            return;

        var user = await userRepository.FindByIdAsync(notification.UserId);
        if (user?.PhoneNumber is null)
            return;
        message = $"درخواست شما با کد رهگیری {notification.Password} و رمز {notification.TrackingNumber} ثبت شد.";

        await communicationService.SendAsync(user.PhoneNumber, message);
    }
}
