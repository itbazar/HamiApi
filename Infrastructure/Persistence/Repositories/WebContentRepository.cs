using Application.Common.Interfaces.Persistence;
using Domain.Models.WebContents;

namespace Infrastructure.Persistence.Repositories;

public class WebContentRepository : GenericRepository<WebContent>, IWebContentRepository
{
    public WebContentRepository(ApplicationDbContext context) : base(context)
    {
    }
}
