using server.Context;
using server.Entities;

namespace server.Repository;

public class TaskHistoryRepository : Repository<TaskHistory>
{
    public TaskHistoryRepository(SupabaseContext context) : base(context)
    {
    }
}