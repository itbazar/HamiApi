using Domain.Models.ComplaintAggregate.Events;

namespace Application.Complaints.Commands.AddComplaintCommand;

internal sealed class ComplaintCreatedDomainEventHandler 
    : INotificationHandler<ComplaintCreatedDomainEvent>
{
    public Task Handle(ComplaintCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
