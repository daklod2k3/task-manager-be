using server.Entities;
using server.Helpers;
using System.Linq.Expressions;

namespace server.Interfaces;

public interface ITaskRepository : IRepository<TaskEntity>
{
    public IEnumerable<TaskEntity> GetTaskByIdUser(Guid id, Expression<Func<TaskEntity, bool>>? filter,
        string? includeProperties, Pagination? pagination);
}