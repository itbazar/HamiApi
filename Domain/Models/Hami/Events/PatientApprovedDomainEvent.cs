using Domain.Primitives;

namespace Domain.Models.Hami.Events;

public record PatientApprovedDomainEvent(
    Guid userGroupId,
    string UserId,
    string PhoneNumber) : DomainEvent(userGroupId);

