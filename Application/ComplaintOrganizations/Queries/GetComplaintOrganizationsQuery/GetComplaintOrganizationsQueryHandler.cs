using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintOrganizations.Queries.GetComplaintOrganizationQuery;

internal class GetComplaintOrganizationsQueryHandler(IComplaintOrganizationRepository organizationRepository) : IRequestHandler<GetComplaintOrganizationsQuery, Result<List<ComplaintOrganization>>>
{
    public async Task<Result<List<ComplaintOrganization>>> Handle(GetComplaintOrganizationsQuery request, CancellationToken cancellationToken)
    {
        var result = await organizationRepository.GetAsync(cc => cc.IsDeleted == false);
        return result.ToList();
    }
}
