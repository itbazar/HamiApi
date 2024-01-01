using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintOrganizations.Commands.AddComplaintOrganization;

internal class AddComplaintOrganizationCommandHandler : IRequestHandler<AddComplaintOrganizationCommand, ComplaintOrganization>
{
    private readonly IComplaintOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddComplaintOrganizationCommandHandler(IComplaintOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ComplaintOrganization> Handle(AddComplaintOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organization = ComplaintOrganization.Create(request.Title, request.Description);
        _organizationRepository.Insert(organization);
        await _unitOfWork.SaveAsync();
        return organization;
    }
}
