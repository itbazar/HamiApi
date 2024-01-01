using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Commands.EditComplaintCategory;

internal class EditComplaintOrganizationCommandHandler : IRequestHandler<EditComplaintOrganizationCommand, ComplaintOrganization>
{
    private readonly IComplaintOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditComplaintOrganizationCommandHandler(IComplaintOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ComplaintOrganization> Handle(EditComplaintOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetSingleAsync(cc => cc.Id == request.Id);
        if (organization is null)
            throw new Exception("Not found!");
        organization.Update(request.Title, request.Description);
        _organizationRepository.Update(organization);
        await _unitOfWork.SaveAsync();
        return organization;
    }
}
