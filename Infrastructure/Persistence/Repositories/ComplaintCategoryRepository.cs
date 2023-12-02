using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;

namespace Infrastructure.Persistence.Repositories;

public class ComplaintCategoryRepository : GenericRepository<ComplaintCategory>, IComplaintCategoryRepository
{
    public ComplaintCategoryRepository(ApplicationDbContext context) : base(context)
    {
    }
}
