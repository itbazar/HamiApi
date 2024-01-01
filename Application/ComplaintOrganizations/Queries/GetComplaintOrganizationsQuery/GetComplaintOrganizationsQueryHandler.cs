using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintOrganizations.Queries.GetComplaintOrganizationQuery;

internal class GetComplaintOrganizationsQueryHandler : IRequestHandler<GetComplaintOrganizationsQuery, List<ComplaintOrganization>>
{
    private readonly IComplaintOrganizationRepository _organizationRepository;

    public GetComplaintOrganizationsQueryHandler(IComplaintOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    public async Task<List<ComplaintOrganization>> Handle(GetComplaintOrganizationsQuery request, CancellationToken cancellationToken)
    {
        var result = await _organizationRepository.GetAsync(cc => cc.IsDeleted == false);
        return result.ToList();
    }
}
