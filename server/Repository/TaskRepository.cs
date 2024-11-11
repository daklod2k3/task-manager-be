using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class TaskRepository : Repository<TaskEntity>, ITaskRepository
{
    public TaskRepository(SupabaseContext context) : base(context)
    {
    }
}