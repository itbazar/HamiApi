using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintOrganizations.Commands.DeleteComplaintOrganization;

public record DeleteComplaintOrganizationCommand(Guid Id, bool IsDeleted) : IRequest<Result<ComplaintOrganization>>;
