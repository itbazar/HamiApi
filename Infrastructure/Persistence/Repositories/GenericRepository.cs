using System.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Application.Common.Interfaces.Persistence;

namespace Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    internal ApplicationDbContext context;
    internal DbSet<TEntity> dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        this.context = context;
        dbSet = context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        bool trackChanges = true,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
    {
        var query = createQuery(filter, trackChanges, includeProperties);

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }

    public virtual async Task<TEntity?> GetFirstAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        bool trackChanges = true,
        string includeProperties = "")
    {
        var query = createQuery(filter, trackChanges, includeProperties);
        return await query.FirstOrDefaultAsync();
    }

    public virtual async Task<TEntity?> GetSingleAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        bool trackChanges = true,
        string includeProperties = "")
    {
        var query = createQuery(filter, trackChanges, includeProperties);
        return await query.SingleOrDefaultAsync();
    }

    public virtual async Task<PagedList<TEntity>> GetPagedAsync(
        PagingInfo paging,
        Expression<Func<TEntity, bool>>? filter = null,
        bool trackChanges = true,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
    {
        var query = createQuery(filter, trackChanges, includeProperties);

        if (orderBy != null)
        {
            return await PagedList<TEntity>.ToPagedList(orderBy(query), paging.PageNumber, paging.PageSize);
        }
        else if (paging.OrderBy != null && !string.IsNullOrEmpty(paging.OrderBy.Column) && !string.IsNullOrEmpty(paging.OrderBy.Type))
        {
            return await PagedList<TEntity>.ToPagedList(getOrderBy(paging)(query), paging.PageNumber, paging.PageSize);
        }
        else
        {
            return await PagedList<TEntity>.ToPagedList(query, paging.PageNumber, paging.PageSize);
        }
    }

    public virtual IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        bool trackChanges = true,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
    {
        var query = createQuery(filter, trackChanges, includeProperties);

        if (orderBy != null)
        {
            return orderBy(query).ToList();
        }
        else
        {
            return query.ToList();
        }
    }

    public virtual TEntity? Find(object id)
    {
        return dbSet.Find(id);
    }

    public virtual async Task<TEntity?> FindAsync(object id)
    {
        return await dbSet.FindAsync(id);
    }

    public virtual void Insert(TEntity entity)
    {
        dbSet.Add(entity);
    }

    public virtual void Delete(object id)
    {
        TEntity? entityToDelete = dbSet.Find(id);
        if(entityToDelete is null) 
        {
            return;
        }
        Delete(entityToDelete);
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        if (context.Entry(entityToDelete).State == EntityState.Detached)
        {
            dbSet.Attach(entityToDelete);
        }
        dbSet.Remove(entityToDelete);
    }

    public virtual void Update(TEntity entityToUpdate)
    {
        dbSet.Attach(entityToUpdate);
        context.Entry(entityToUpdate).State = EntityState.Modified;
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.CountAsync();
    }

    public virtual int Count(
        Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query.Count();
    }

    private Expression<Func<TEntity, bool>> makeFilter(PagingInfo paging)
    {
        var item = Expression.Parameter(typeof(TEntity), "item");
        if (paging == null || paging.QueryItem == null)
        {
            var result = Expression.Lambda<Func<TEntity, bool>>(Expression.IsTrue(Expression.Constant(true)), item);
            return result;
        }


        var queryItem = paging.QueryItem;
        var props = queryItem.Column.Split(',');
        MemberExpression prop;
        var falseConstant = Expression.Constant(false);
        BinaryExpression orExp = Expression.Or(falseConstant, falseConstant);
        ConstantExpression value;
        MethodInfo? method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        if(method is null)
        {
            //TODO: Handle this error
            throw new Exception();
        }
        MethodCallExpression containsMethodExp;
        foreach (var propStr in props)
        {
            prop = Expression.Property(item, propStr);
            value = Expression.Constant(queryItem.Value);
            containsMethodExp = Expression.Call(prop, method, value);
            orExp = Expression.Or(orExp, containsMethodExp);
        }

        var lambda = Expression.Lambda<Func<TEntity, bool>>(orExp, item);

        return lambda;
    }

    private Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> getOrderBy(PagingInfo paging)
    {
        string orderColumn = paging.OrderBy!.Column;
        string orderType = paging.OrderBy.Type;

        Type typeQueryable = typeof(IQueryable<TEntity>);
        ParameterExpression argQueryable = Expression.Parameter(typeQueryable, "p");
        var outerExpression = Expression.Lambda(argQueryable, argQueryable);
        string[] props = orderColumn.Split('.');
        IQueryable<TEntity> query = new List<TEntity>().AsQueryable();
        Type type = typeof(TEntity);
        ParameterExpression arg = Expression.Parameter(type, "x");

        Expression expr = arg;
        foreach (string prop in props)
        {
            PropertyInfo? pi = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if(pi is null)
            {
                //TODO: Handle this error
                throw new Exception();
            }
            expr = Expression.Property(expr, pi);
            type = pi.PropertyType;
        }
        LambdaExpression lambda = Expression.Lambda(expr, arg);
        string methodName = orderType == "asc" ? "OrderBy" : "OrderByDescending";

        MethodCallExpression resultExp =
            Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(TEntity), type }, outerExpression.Body, Expression.Quote(lambda));
        var finalLambda = Expression.Lambda(resultExp, argQueryable);
        return (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)finalLambda.Compile();
    }

    private IQueryable<TEntity> createQuery(Expression<Func<TEntity, bool>>? filter, bool trackChanges, string includeProperties)
    {
        IQueryable<TEntity> query = dbSet;
        if (trackChanges)
            query = query.AsTracking();
        else
            query = query.AsNoTracking();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        return query;
    }
}