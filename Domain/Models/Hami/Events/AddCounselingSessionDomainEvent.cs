using Domain.Primitives;

namespace Domain.Models.Hami.Events;

public record AddCounselingSessionDomainEvent(
    Guid userGroupId,
    string UserId,
    DateTime ScheduledDate,
    string Topic,
    string MeetingLink) : DomainEvent(userGroupId);

