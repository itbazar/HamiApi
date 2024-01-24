using Domain.Primitives;

namespace Domain.Models.ComplaintAggregate.Events;

public record ComplaintUpdatedDomainEvent(Guid Id, Guid ComplaintId) : DomainEvent(Id);

