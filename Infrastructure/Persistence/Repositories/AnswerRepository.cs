using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Infrastructure.Persistence.Repositories;

public class AnswerRepository : GenericRepository<Answer>, IAnswerRepository
{
    public AnswerRepository(ApplicationDbContext context) : base(context)
    {
    }
}
