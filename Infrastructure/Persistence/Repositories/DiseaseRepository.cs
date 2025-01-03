namespace Infrastructure.Persistence.Repositories;

using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

public class DiseaseRepository : GenericRepository<Disease>, IDiseaseRepository
{
    public DiseaseRepository(ApplicationDbContext context) : base(context)
    {
    }
}
