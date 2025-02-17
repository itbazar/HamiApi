using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Infrastructure.Persistence.Repositories;

public class PatientLabTestRepository : GenericRepository<PatientLabTest>, IPatientLabTestRepository
{
    public PatientLabTestRepository(ApplicationDbContext context) : base(context)
    {
    }
}


