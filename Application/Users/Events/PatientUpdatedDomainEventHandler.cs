using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using Domain.Models.ComplaintAggregate.Events;
using SharedKernel.Statics;

namespace Application.Complaints.Events;

//internal sealed class ComplaintUpdatedDomainEventHandler(
//    IUserRepository userRepository,
//    ICommunicationService communicationService) : INotificationHandler<ComplaintUpdatedDomainEvent>
//{
//    public async Task Handle(ComplaintUpdatedDomainEvent notification, CancellationToken cancellationToken)
//    {
//        if(notification.Actor == Actor.Inspector)
//        {
//            if(notification.State == ComplaintState.WaitingForCitizenResponse)
//            {
//                if (notification.UserId is null)
//                    return;
//                var user = await userRepository.FindByIdAsync(notification.UserId);
//                if (user?.PhoneNumber is null)
//                    return;
//                var message = $"لطفاً جهت تکمیل اطلاعات مربوط به درخواست با کد رهگیری {notification.TrackingNumber} به سامانه عیون مراجعه نمایید.";

//                await communicationService.SendAsync(user.PhoneNumber, message);
//                await communicationService.SendNotification(
//                    user.Id,
//                    "Updated",
//                    message,
//                    notification.ComplaintId);
//            }
//        }
//        else
//        {
//            if(notification.State == ComplaintState.CitizenReplied)
//            {
//                var inspectors = await userRepository.GetUsersInRole(RoleNames.Inspector);
//                var message = $"درخواست با کد رهگیری {notification.TrackingNumber} توسط شهروند پاسخ داده شد.";
//                foreach (var inspector in inspectors)
//                {
//                    if (inspector?.PhoneNumber is null)
//                        continue;
//                    await communicationService.SendAsync(inspector.PhoneNumber, message);
//                    await communicationService.SendNotification(
//                        inspector.Id,
//                        "Updated",
//                        message,
//                        notification.ComplaintId);
//                }
//            }
//        }
//    }
//}
