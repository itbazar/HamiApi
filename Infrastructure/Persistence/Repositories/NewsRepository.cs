using Application.Common.Interfaces.Persistence;
using Domain.Models.News;

namespace Infrastructure.Persistence.Repositories;

public class NewsRepository : GenericRepository<News>, INewsRepository
{
    public NewsRepository(ApplicationDbContext context) : base(context)
    {
    }
}
