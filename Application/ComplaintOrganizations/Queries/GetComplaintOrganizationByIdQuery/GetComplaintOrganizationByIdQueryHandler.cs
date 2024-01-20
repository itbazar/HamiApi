using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;

namespace Application.ComplaintOrganizations.Queries.GetComplaintOrganizationByIdQuery;

internal class GetComplaintOrganizationByIdQueryHandler(IComplaintOrganizationRepository organizationRepository) : IRequestHandler<GetComplaintOrganizationByIdQuery, Result<ComplaintOrganization>>
{
    public async Task<Result<ComplaintOrganization>> Handle(GetComplaintOrganizationByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await organizationRepository.GetSingleAsync(cc => cc.IsDeleted == false);
        if (result is null)
            return GenericErrors.NotFound;
        return result;
    }
}
