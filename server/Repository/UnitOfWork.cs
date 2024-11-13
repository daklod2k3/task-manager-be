using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly SupabaseContext _context;

    public UnitOfWork(SupabaseContext context)
    {
        _context = context;
        Task = new TaskRepository(_context);
        TaskDepartment = new TaskDepartmentRepository(_context);
        TaskUser = new TaskUserRepository(_context);
        User = new UserRepository(_context);
        Department = new DepartmentRepository(_context);
    }

    public ITaskRepository Task { get; }
    public ITaskDepartmentRepository TaskDepartment { get; }
    public ITaskUserRepository TaskUser { get; }
    public IUserRepository User { get; }
    public IDepartmentRepository Department {  get; }

    public int Save()
    {
        return _context.SaveChanges();
    }
}