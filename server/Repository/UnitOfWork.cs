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
        Tasks = new TaskRepository(_context);
        TaskDepartments = new TaskDepartmentRepository(_context);
        TaskUsers = new TaskUserRepository(_context);
        Users = new UserRepository(_context);
        Departments = new DepartmentRepository(_context);
        DepartmentUsers = new DepartmentUserRepository(_context);
        Roles = new RoleRepository(_context);
        Resources = new ResourceRepository(_context);
        TaskComments = new TaskCommentRepository(_context);
        Files = new FileRepository(_context);
        Channels = new ChannelRepository(_context);
        ChannelMessages = new ChannelMessageRepository(_context);
        ChannelUsers = new ChannelUserRepository(_context);
        Notifications = new NotificationRepository(_context);
        UserMessages = new UserMessageRepository(_context);
        TaskHistories = new TaskHistoryRepository(_context);
    }


    public IChannelRepository Channels { get; }
    public IChannelMessageRepository ChannelMessages { get; }
    public IChannelUserRepository ChannelUsers { get; }
    public IDepartmentRepository Departments { get; }
    public IDepartmentUserRepository DepartmentUsers { get; }
    public IFileRepository Files { get; }
    public INotificationRepository Notifications { get; }
    public IRoleRepository Roles { get; }
    public ITaskCommentRepository TaskComments { get; }
    public ITaskDepartmentRepository TaskDepartments { get; }
    public ITaskRepository Tasks { get; }
    public ITaskHistoryRepository TaskHistories { get; }
    public ITaskUserRepository TaskUsers { get; }
    public IUserMessageRepository UserMessages { get; }
    public IUserRepository Users { get; }

    public IResourceRepository Resources { get; }

    public int Save()
    {
        return _context.SaveChanges();
    }
}