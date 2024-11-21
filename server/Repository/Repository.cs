using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using server.Context;
using server.Helpers;
using server.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace server.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    // private readonly SupabaseContext _context;
    protected readonly DbSet<T> DbSet;
    private readonly SupabaseContext _context;

    public Repository(SupabaseContext context)
    {
        _context = context;
        DbSet = context.Set<T>();
    }

    public T Add(T entity)
    {
        return DbSet.Add(entity).Entity;
    }

    public bool Any(Expression<Func<T, bool>> filter)
    {
        return DbSet.Any(filter);
    }


    public T Get(Expression<Func<T, bool>>? filter, string? includeProperties)
    {
        return GetQuery(filter, includeProperties).FirstOrDefault();
    }

    public IEnumerable<T> GetAll( Expression<Func<T, bool>>? filter, string? includeProperties)
    {
        return GetQuery(filter, includeProperties).ToList();
    }

    public virtual T GetById(string id, string? includeProperties = "*", string? keyProperty = "id")
    {
        var entity = DbSet.FirstOrDefault(e=> EF.Property<long>(e, keyProperty) == long.Parse(id));
        return entity;
    }

    public T Remove(T entity)
    {
        return DbSet.Remove(entity).Entity;
    }

    public T Update(T entity)
    {
        return DbSet.Update(entity).Entity;
    }

    public T UpdatePatch(string id, JsonPatchDocument<T> patch)
    {
        var entity = DbSet.Find(id);
        patch.ApplyTo(entity);
        Save();
        return entity;
    }

    public IQueryable<T> GetQuery(Expression<Func<T, bool>>? filter, string? includeProperties)
    {
        IQueryable<T> query = DbSet;

        if (filter != null) query = query.Where(filter);

        if (!string.IsNullOrEmpty(includeProperties))
            foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProp);

        return query;
    }

    public int Save()
    {
        return _context.SaveChanges();
    }
}