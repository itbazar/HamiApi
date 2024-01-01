using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;

namespace Infrastructure.Persistence.Repositories;

public class ComplaintOrganizationRepository : GenericRepository<ComplaintOrganization>, IComplaintOrganizationRepository
{
    public ComplaintOrganizationRepository(ApplicationDbContext context) : base(context)
    {
    }
}
