using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintOrganizations.Queries.GetComplaintOrganizationByIdQuery;

internal class GetComplaintOrganizationByIdQueryHandler : IRequestHandler<GetComplaintOrganizationByIdQuery, ComplaintOrganization>
{
    private readonly IComplaintOrganizationRepository _organizationRepository;

    public GetComplaintOrganizationByIdQueryHandler(IComplaintOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    public async Task<ComplaintOrganization> Handle(GetComplaintOrganizationByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _organizationRepository.GetSingleAsync(cc => cc.IsDeleted == false);
        if (result is null)
            throw new Exception("Not found!");
        return result;
    }
}
