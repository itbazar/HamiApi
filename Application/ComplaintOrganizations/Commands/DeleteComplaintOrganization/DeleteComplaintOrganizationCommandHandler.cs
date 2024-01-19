using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintOrganizations.Commands.DeleteComplaintOrganization;

internal class DeleteComplaintOrganizationCommandHandler(IComplaintOrganizationRepository organizationRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteComplaintOrganizationCommand, Result<ComplaintOrganization>>
{
    public async Task<Result<ComplaintOrganization>> Handle(DeleteComplaintOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organization = await organizationRepository.GetSingleAsync(cc => cc.Id == request.Id);
        if (organization is null)
            throw new Exception("Not found!");
        organization.Delete(request.IsDeleted);
        organizationRepository.Update(organization);
        await unitOfWork.SaveAsync();
        return organization;
    }
}
