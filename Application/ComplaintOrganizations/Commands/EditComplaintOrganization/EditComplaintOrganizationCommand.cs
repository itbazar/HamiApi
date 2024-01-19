using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Commands.EditComplaintCategory;

public record EditComplaintOrganizationCommand(Guid Id, string? Title, string? Description)
: IRequest<Result<ComplaintOrganization>>;
