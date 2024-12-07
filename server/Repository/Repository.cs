using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Communications.Common;
using server.Context;
using server.Helpers;
using server.Interfaces;

namespace server.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    // private readonly SupabaseContext _context;
    internal SupabaseContext context;

    internal DbSet<T> dbSet;

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

    public T GetById(object id, string includeProperties = "", string? keyProperty = "Id")
    {
        IQueryable<T> query = dbSet;
        if (includeProperties != null)
            foreach (var includeProperty in includeProperties.Split
                         (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);
        var entity = query.FirstOrDefault(e => EF.Property<long>(e, keyProperty).ToString() == id.ToString());
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
        var entity = dbSet.Find(id);
        patch.ApplyTo(entity);
        Save();
        return entity;
    }

    public int Save()
    {
        return context.SaveChanges();
    }

    public IEnumerable<T> Get(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
        string? orderBy = null,
        int? page = null, int? pageSize = null)
    {
        return GetQuery(filter, includeProperties, orderBy, page, pageSize).ToList();
    }

    public IQueryable<T> GetQuery(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
        string? orderBy = null,
        int? page = null, int? pageSize = null)
    {
        IQueryable<T> query = dbSet;

        if (filter != null) query = query.Where(filter);

        query = query.GetInclude(includeProperties);

        query = query.GetOrderBy(orderBy);

        return query.Paginate(page, pageSize);
    }
}