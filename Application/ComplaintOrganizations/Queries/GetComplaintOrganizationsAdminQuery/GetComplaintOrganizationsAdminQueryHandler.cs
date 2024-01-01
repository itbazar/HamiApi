using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintOrganizations.Queries.GetComplaintCategoriesAdminQuery;

internal class GetComplaintOrganizationsAdminQueryHandler : IRequestHandler<GetComplaintOrganizationsAdminQuery, List<ComplaintOrganization>>
{
    private readonly IComplaintOrganizationRepository _organizationRepository;

    public GetComplaintOrganizationsAdminQueryHandler(IComplaintOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    public async Task<List<ComplaintOrganization>> Handle(GetComplaintOrganizationsAdminQuery request, CancellationToken cancellationToken)
    {
        var result = await _organizationRepository.GetAsync();
        return result.ToList();
    }
}
