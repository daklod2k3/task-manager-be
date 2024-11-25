using server.Context;
using server.Entities;
using server.Helpers;
using server.Interfaces;
using System.Linq.Expressions;

namespace server.Repository;

public class TaskRepository : Repository<TaskEntity>, ITaskRepository
{
    public TaskRepository(SupabaseContext context) : base(context)
    {
    }
    public IEnumerable<TaskEntity> GetTaskByIdUser(Guid id, Expression<Func<TaskEntity, bool>>? filter,string? orderBy,
        string? includeProperties, Pagination? pagination)
    {
        if (Guid.Empty == id) return Enumerable.Empty<TaskEntity>();
        filter ??= t => true;
        var query = GetQuery(filter, orderBy, includeProperties)
            .Where(t => t.TaskUsers.Any(tu => tu.UserId == id))
            .Distinct();
        return query.Paginate(pagination).ToList();
    }
}