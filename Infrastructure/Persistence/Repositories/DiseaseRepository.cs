namespace Infrastructure.Persistence.Repositories;

using Application.Common.Interfaces.Persistence;
using Domain.Models.DiseaseAggregate;

public class DiseaseRepository : GenericRepository<Disease>, IDiseaseRepository
{
    public DiseaseRepository(ApplicationDbContext context) : base(context)
    {
    }
}
