using Application.Common.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private ApplicationDbContext context;
    public UnitOfWork(ApplicationDbContext context)
    {
        this.context = context;
    }

    public DbContext DbContext { get { return context; } }


    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }

    public void Save()
    {
        context.SaveChanges();
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}