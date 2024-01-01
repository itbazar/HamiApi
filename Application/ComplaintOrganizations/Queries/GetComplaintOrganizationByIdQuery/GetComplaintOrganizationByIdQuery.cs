using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintOrganizations.Queries.GetComplaintOrganizationByIdQuery;

public record GetComplaintOrganizationByIdQuery(Guid Id) : IRequest<ComplaintOrganization>;
