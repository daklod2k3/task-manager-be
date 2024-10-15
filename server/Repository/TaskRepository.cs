using server.Context;
using server.Interfaces;
using server.Entities;
using System.Linq.Expressions;

namespace server.Repository
{
    public class TaskRepository : Repository<Tasks>, ITaskRepository
    {
        private readonly SupabaseContext _context;
        public TaskRepository(SupabaseContext context) : base(context)
        {
            _context = context;

        }
        public void Update(Tasks task)
        {
            _context.Tasks.Update(task);
        }

       
    }
}
