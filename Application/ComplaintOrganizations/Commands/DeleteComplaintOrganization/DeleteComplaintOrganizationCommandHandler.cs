using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintOrganizations.Commands.DeleteComplaintOrganization;

internal class DeleteComplaintOrganizationCommandHandler : IRequestHandler<DeleteComplaintOrganizationCommand, ComplaintOrganization>
{
    private readonly IComplaintOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteComplaintOrganizationCommandHandler(IComplaintOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ComplaintOrganization> Handle(DeleteComplaintOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetSingleAsync(cc => cc.Id == request.Id);
        if (organization is null)
            throw new Exception("Not found!");
        organization.Delete(request.IsDeleted);
        _organizationRepository.Update(organization);
        await _unitOfWork.SaveAsync();
        return organization;
    }
}
