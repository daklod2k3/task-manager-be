using server.Context;
using server.Interfaces;

namespace server.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SupabaseContext _context;
        public ITaskRepository Task {  get; private set; }

        public UnitOfWork(SupabaseContext context) 
        { 
            _context = context;
            Task = new TaskRepository(_context);

        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
