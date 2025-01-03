using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Infrastructure.Persistence.Repositories;

public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
{
    public QuestionRepository(ApplicationDbContext context) : base(context)
    {
    }
}
