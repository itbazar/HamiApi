using Domain.Primitives;

namespace Domain.Models.Hami.Events;

public record PatientRejectedDomainEvent(
    Guid userGroupId,
    string UserId,
    string RejectResion,
    string PhoneNumber) : DomainEvent(userGroupId);

