using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class TaskCommentRepository : Repository<TaskComment>, ITaskCommentRepository
{
    public TaskCommentRepository(SupabaseContext context) : base(context)
    {
    }
}