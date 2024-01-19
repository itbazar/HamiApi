using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintOrganizations.Commands.AddComplaintOrganization;

internal class AddComplaintOrganizationCommandHandler(IComplaintOrganizationRepository organizationRepository, IUnitOfWork unitOfWork) : IRequestHandler<AddComplaintOrganizationCommand, Result<ComplaintOrganization>>
{
    public async Task<Result<ComplaintOrganization>> Handle(AddComplaintOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organization = ComplaintOrganization.Create(request.Title, request.Description);
        organizationRepository.Insert(organization);
        await unitOfWork.SaveAsync();
        return organization;
    }
}
