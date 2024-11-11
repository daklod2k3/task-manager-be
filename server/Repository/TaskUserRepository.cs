using Microsoft.EntityFrameworkCore.ChangeTracking;
using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository
{
    public class TaskUserRepository : Repository<TaskUser>, ITaskUserRepository
    {
        private readonly SupabaseContext _context;
        public TaskUserRepository(SupabaseContext context) : base(context)
        {
            _context = context;

        }
        public TaskUser Update(TaskUser taskUser)
        {
            return _context.TaskUsers.Update(taskUser).Entity;
        }


    }
}
