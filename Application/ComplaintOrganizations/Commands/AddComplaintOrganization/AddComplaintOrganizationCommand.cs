using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintOrganizations.Commands.AddComplaintOrganization;

public record AddComplaintOrganizationCommand(string Title, string Description) 
    : IRequest<Result<ComplaintOrganization>>;
