using server.Context;
using server.Interfaces;
using server.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace server.Repository
{
    public class TaskRepository : Repository<Tasks>, ITaskRepository
    {
        private readonly SupabaseContext _context;
        public TaskRepository(SupabaseContext context) : base(context)
        {
            _context = context;
            dbSet = _context.Set<Tasks>();

        }

        
        public Tasks Update(Tasks task)
        {
            return _context.Tasks.Update(task).Entity;
        }

       
    }
}
