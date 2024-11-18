using server.Helpers;
using System.Linq.Expressions;

namespace server.Interfaces;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string includeProperties = null);
    T Get(Expression<Func<T, bool>>? filter, string includeProperties = null);
    IQueryable<T> GetQuery(Expression<Func<T, bool>>? filter, string? includeProperties = null);
    T GetById(int id);
    T Add(T entity);
    bool Any(Expression<Func<T, bool>> filter);
    T Remove(T entity);
    T Update(T entity);
}