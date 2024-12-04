using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Communications.Common;
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

        if (includeProperties != null)
            foreach (var includeProperty in includeProperties.Split
                         (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

        // if (orderBy != null)
        Console.WriteLine("tesst");
        // return query.OrderBy(x => EF.Property<object>(x, "Status")).ToList();
        return query.ToList();
    }

    public virtual T GetById(object id, string includeProperties = "", string? keyProperty = "id")
    {
        IQueryable<T> query = dbSet;
        if (includeProperties != null)
            foreach (var includeProperty in includeProperties.Split
                         (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);
        var entity = query.First(e => EF.Property<long>(e, keyProperty).ToString() == id.ToString());
        return entity;
    }

    public T Remove(T entity)
    {
        return dbSet.Remove(entity).Entity;
    }

    public T Update(T entity)
    {
        var origin = dbSet.Find(entity.GetPropertyUsingReflection("Id"));
        context.Entry(origin).CurrentValues.SetValues(entity);
        return origin;
    }

    public T UpdatePatch(object id, JsonPatchDocument<T> patch)
    {
        var entity = GetById(id);
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