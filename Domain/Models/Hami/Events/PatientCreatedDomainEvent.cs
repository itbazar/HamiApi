using Domain.Primitives;

namespace Domain.Models.Hami.Events;

public record PatientCreatedDomainEvent(
    Guid userMedicalId,
    string UserId,
    string PhoneNumber) : DomainEvent(userMedicalId);

