using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces.Persistence;

public interface IUnitOfWork
{
    public void Save();
    public Task SaveAsync();
    public DbContext DbContext { get; }
}
