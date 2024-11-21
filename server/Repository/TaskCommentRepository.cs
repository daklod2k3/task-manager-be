using server.Context;
using server.Entities;

namespace server.Repository;

public class TaskCommentRepository: Repository<TaskComment>
{
    public TaskCommentRepository(SupabaseContext context) : base(context)
    {
    }
}