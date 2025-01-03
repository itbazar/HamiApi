using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Infrastructure.Persistence.Repositories;

public class PatientGroupRepository : GenericRepository<PatientGroup>, IPatientGroupRepository
{
    public PatientGroupRepository(ApplicationDbContext context) : base(context)
    {
    }
}
