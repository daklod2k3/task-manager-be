using System.Linq.Expressions;
using server.Context;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Repository;

public class TaskRepository : Repository<TaskEntity>, ITaskRepository
{
    public TaskRepository(SupabaseContext context) : base(context)
    {
    }

    public IEnumerable<TaskEntity> GetTaskByIdUser(Guid id, Expression<Func<TaskEntity, bool>>? filter, string? orderBy,
        string? includeProperties, QueryableExtensions.Pagination? pagination)
    {
        if (Guid.Empty == id) return Enumerable.Empty<TaskEntity>();
        filter ??= t => true;
        var query = GetQuery(filter, orderBy, includeProperties, pagination.Page, pagination.PageSize)
            .Where(t => t.TaskUsers.Any(tu => tu.UserId == id))
            .Distinct();
        return query.ToList();
    }

    public IQueryable<TaskEntity> GetByDepartmentId(long id)
    {
        return dbSet.Where(t=> t.TaskDepartments.Any(td=> td.DepartmentId == id));
    }
}