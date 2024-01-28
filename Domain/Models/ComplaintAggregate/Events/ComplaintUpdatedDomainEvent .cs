using Domain.Primitives;

namespace Domain.Models.ComplaintAggregate.Events;

public record ComplaintUpdatedDomainEvent(
    Guid Id,
    Guid ComplaintId,
    string TrackingNumber,
    Actor Actor,
    ComplaintState State,
    string? UserId) : DomainEvent(Id);

