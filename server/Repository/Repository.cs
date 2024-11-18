using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using server.Context;
using server.Helpers;
using server.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace server.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    // private readonly SupabaseContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(SupabaseContext context)
    {
        // _context = context;
        _dbSet = context.Set<T>();
    }

    public T Add(T entity)
    {
        return _dbSet.Add(entity).Entity;
    }

    public bool Any(Expression<Func<T, bool>> filter)
    {
        return _dbSet.Any(filter);
    }


    public T Get(Expression<Func<T, bool>>? filter, string? includeProperties)
    {
        return GetQuery(filter, includeProperties).FirstOrDefault();
    }

    public IEnumerable<T> GetAll( Expression<Func<T, bool>>? filter, string? includeProperties)
    {
        return GetQuery(filter, includeProperties).ToList();
    }

    public virtual T GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public T Remove(T entity)
    {
        return _dbSet.Remove(entity).Entity;
    }

    public T Update(T entity)
    {
        return _dbSet.Update(entity).Entity;
    }

    public IQueryable<T> GetQuery(Expression<Func<T, bool>>? filter, string? includeProperties)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null) query = query.Where(filter);

        if (!string.IsNullOrEmpty(includeProperties))
            foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProp);

        return query;
    }
}