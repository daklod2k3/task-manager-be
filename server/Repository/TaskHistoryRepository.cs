using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository
{
    public class TaskHistoryRepository : Repository<TaskHistory>, ITaskHistoryRepository
    {
        private readonly SupabaseContext _context;
        public TaskHistoryRepository(SupabaseContext context) : base(context)
        {
            _context = context;

        }
        
        public TaskHistory Update(TaskHistory taskHistory)
        {
            return _context.TaskHistories.Update(taskHistory).Entity;
        }
    }

}