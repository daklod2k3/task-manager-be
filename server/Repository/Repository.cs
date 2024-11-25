using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using server.Context;
using server.Interfaces;

namespace server.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    internal SupabaseContext context;

    internal DbSet<T> dbSet;
    // private readonly SupabaseContext _context;

    public Repository(SupabaseContext context)
    {
        this.context = context;
        dbSet = context.Set<T>();
    }

    public T Add(T entity)
    {
        return dbSet.Add(entity).Entity;
    }

    public bool Any(Expression<Func<T, bool>> filter)
    {
        return dbSet.Any(filter);
    }


    public virtual IEnumerable<T> Get(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "")
    {
        IQueryable<T> query = dbSet;

        if (filter != null) query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split
                     (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(includeProperty);

        if (orderBy != null)
            return orderBy(query).ToList();
        return query.ToList();
    }

    public virtual T GetById(object id, string includeProperties = "", string? keyProperty = "id")
    {
        var query = dbSet.AsQueryable();
        if (includeProperties != null) query = query.Include(includeProperties);
        var entity = query
            .FirstOrDefault(e => EF.Property<long>(e, keyProperty).ToString() == id);
        return entity;
    }

    public T Remove(T entity)
    {
        return dbSet.Remove(entity).Entity;
    }

    public T Update(T entity)
    {
        return dbSet.Update(entity).Entity;
    }

    public T UpdatePatch(string id, JsonPatchDocument<T> patch)
    {
        var entity = dbSet.Find(id);
        patch.ApplyTo(entity);
        Save();
        return entity;
    }

    public IQueryable<T> GetQuery(Expression<Func<T, bool>>? filter, string? includeProperties)
    {
        IQueryable<T> query = dbSet;

        if (filter != null) query = query.Where(filter);

        if (!string.IsNullOrEmpty(includeProperties))
            foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProp);

        return query;
    }

    public int Save()
    {
        return context.SaveChanges();
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties)
    {
        return GetQuery(filter, includeProperties).ToList();
    }
}