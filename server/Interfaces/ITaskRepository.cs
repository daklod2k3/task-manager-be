using System.Linq.Expressions;
using server.Entities;
using server.Helpers;

namespace server.Interfaces;

public interface ITaskRepository : IRepository<TaskEntity>
{
    public IEnumerable<TaskEntity> GetTaskByIdUser(Guid id, Expression<Func<TaskEntity, bool>>? filter, string? orderBy,
        string? includeProperties, QueryableExtensions.Pagination? pagination);
}