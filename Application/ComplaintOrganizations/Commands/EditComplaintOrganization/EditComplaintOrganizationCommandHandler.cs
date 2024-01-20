using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;

namespace Application.ComplaintCategories.Commands.EditComplaintCategory;

internal class EditComplaintOrganizationCommandHandler(IComplaintOrganizationRepository organizationRepository, IUnitOfWork unitOfWork) : IRequestHandler<EditComplaintOrganizationCommand, Result<ComplaintOrganization>>
{
    public async Task<Result<ComplaintOrganization>> Handle(EditComplaintOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organization = await organizationRepository.GetSingleAsync(cc => cc.Id == request.Id);
        if (organization is null)
            return GenericErrors.NotFound;
        organization.Update(request.Title, request.Description);
        organizationRepository.Update(organization);
        await unitOfWork.SaveAsync();
        return organization;
    }
}
