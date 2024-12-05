using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;

namespace server.Interfaces;

public interface IRepository<T> where T : class
{
    IEnumerable<T> Get(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
        string? orderBy = null, int? page = null, int? pageSize = null);

    IQueryable<T> GetQuery(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
        string? orderBy = null, int? page = 1, int? pageSize = 50);

    T GetById(object id, string? includeProperties = null, string? keyProperty = "Id");
    T Add(T entity);
    bool Any(Expression<Func<T, bool>> filter);
    T Remove(T entity);
    T Update(T entity);
    T UpdatePatch(object id, JsonPatchDocument<T> patch);
    int Save();
}