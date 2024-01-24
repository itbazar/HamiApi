using Domain.Primitives;

namespace Domain.Models.ComplaintAggregate.Events;

public record ComplaintCreatedDomainEvent(Guid Id, Guid ComplaintId) : DomainEvent(Id);

