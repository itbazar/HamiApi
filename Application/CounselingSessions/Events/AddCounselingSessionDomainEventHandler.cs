using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami.Events;
using SharedKernel.Statics;
using System.Globalization; // اضافه کردن این کتابخانه برای تغییر فرمت تاریخ

namespace Application.CounselingSessions.Events;

internal sealed class AddCounselingSessionDomainEventHandler(
    ICommunicationService communicationService,
    IUserRepository userRepository,
    ICounselingSessionRepository counselingSessionRepository,
    IUserGroupMembershipRepository userGroupMembershipRepository
) : INotificationHandler<AddCounselingSessionDomainEvent>
{
    public async Task Handle(AddCounselingSessionDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.UserId is null)
            return;

        // تبدیل تاریخ میلادی به تاریخ شمسی به همراه ساعت
        string formattedDateTime = ConvertToPersianDateTime(notification.ScheduledDate);

        string message = $"جلسه گروهی در تاریخ {notification.ScheduledDate} برای شما برگزار خواهد شد.";
        string message2 = $" لطفا در زمان اعلام شده از طریق لینک  {notification.MeetingLink} وارد جلسه شوید";

        var members = await userGroupMembershipRepository.GetAsync(q => q.PatientGroupId == notification.userGroupId);
        foreach (var item in members.ToList())
        {
            var user = await userRepository.FindByIdAsync(item.UserId);
            if (user?.PhoneNumber is null)
                continue;

            await communicationService.SendAsync(user.PhoneNumber, message + message2);
        }
    }

    /// <summary>
    /// تبدیل تاریخ میلادی به تاریخ و زمان شمسی
    /// </summary>
    /// <param name="dateTime">تاریخ و زمان میلادی</param>
    /// <returns>رشته‌ای از تاریخ و زمان شمسی</returns>
    private string ConvertToPersianDateTime(DateTime dateTime)
    {
        var persianCalendar = new PersianCalendar();

        string persianDate = $"{persianCalendar.GetYear(dateTime)}/{persianCalendar.GetMonth(dateTime):00}/{persianCalendar.GetDayOfMonth(dateTime):00}";
        string time = dateTime.ToString("HH:mm"); // استخراج ساعت و دقیقه (24 ساعته)

        return $"{persianDate} ساعت {time}";
    }
}
