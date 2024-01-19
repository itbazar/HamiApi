using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintOrganizations.Queries.GetComplaintCategoriesAdminQuery;

internal class GetComplaintOrganizationsAdminQueryHandler(IComplaintOrganizationRepository organizationRepository) 
    : IRequestHandler<GetComplaintOrganizationsAdminQuery, Result<List<ComplaintOrganization>>>
{

    public async Task<Result<List<ComplaintOrganization>>> Handle(GetComplaintOrganizationsAdminQuery request, CancellationToken cancellationToken)
    {
        var result = await organizationRepository.GetAsync();
        return result.ToList();
    }
}
