using System.Linq.Expressions;

namespace Application.Common.Interfaces.Persistence;

public interface IGenericRepository<TEntity> where TEntity : class
{
    public IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        bool trackChanges = true,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");
    public Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        bool trackChanges = true,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");
    public Task<TEntity?> GetFirstAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        bool trackChanges = true,
        string includeProperties = "");
    public Task<TEntity?> GetSingleAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        bool trackChanges = true,
        string includeProperties = "");
    public Task<PagedList<TEntity>> GetPagedAsync(
        PagingInfo paging,
        Expression<Func<TEntity, bool>>? filter = null,
        bool trackChanges = true,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");
    public TEntity? Find(object id);
    public Task<TEntity?> FindAsync(object id);
    public void Insert(TEntity entity);
    public void Delete(object id);
    public void Delete(TEntity entityToDelete);
    public void Update(TEntity entityToUpdate);
    public int Count(Expression<Func<TEntity, bool>>? filter = null);
    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null);
}
