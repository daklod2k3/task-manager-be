using server.Context;
using server.Interfaces;

namespace server.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SupabaseContext _context;
        public ITaskRepository Task {  get; private set; }

        public ITaskDepartmentRepository TaskDepartment { get; private set; }

        public ITaskUserRepository TaskUser { get; private set; }
        public UnitOfWork(SupabaseContext context) 
        { 
            _context = context;
            Task = new TaskRepository(_context);
            TaskDepartment = new TaskDepartmentRepository(_context);
            TaskUser = new TaskUserRepository(_context);
           

        }

        public int Save()
        {
            return _context.SaveChanges();

        }
    }
}
