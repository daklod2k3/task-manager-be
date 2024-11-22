using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;

namespace server.Interfaces;

public interface IRepository<T> where T : class
{
    // IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string includeProperties = null);
    IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "");

    IQueryable<T> GetQuery(Expression<Func<T, bool>>? filter, string? includeProperties = null);
    T GetById(object id, string? includeProperties = "*", string? keyProperty = "Id");
    T Add(T entity);
    bool Any(Expression<Func<T, bool>> filter);
    T Remove(T entity);
    T Update(T entity);
    T UpdatePatch(string id, JsonPatchDocument<T> patch);
    int Save();
}