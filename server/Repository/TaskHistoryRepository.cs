using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class TaskHistoryRepository : Repository<TaskHistory>, ITaskHistoryRepository
{
    public TaskHistoryRepository(SupabaseContext context) : base(context)
    {
    }
}