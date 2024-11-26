using server.Context;
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
        DepartmentUser = new DepartmentUserRepository(_context);
        Channel = new ChannelRepository(_context);
        ChannelUser = new ChannelUserRepository(_context);
        UserMessage = new UserMessageRepository(_context);
        Role = new RoleRepository(_context);
        FileRepository = new FileRepository(_context);
    }

    public IFileRepository FileRepository { get; }

    public ITaskRepository Task { get; }
    public ITaskDepartmentRepository TaskDepartment { get; }
    public IDepartment Department { get; }
    public IDepartmentUser DepartmentUser { get; }
    public ITaskUserRepository TaskUser { get; }
    public IUserRepository User { get; }
    //public IDepartmentRepository Department {  get; }
    //public IDepartmentUserRepository DepartmentUser { get; }
    public IChannelRepository Channel { get; }
    public IChannelUserRepository ChannelUser { get; }
    public IUserMessageRepository UserMessage { get; }
    public IRoleRepository Role { get; }

    public int Save()
    {
        return _context.SaveChanges();
    }
}