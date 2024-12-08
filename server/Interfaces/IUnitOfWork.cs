namespace server.Interfaces;

public interface IUnitOfWork
{
    IChannelRepository Channels { get; }
    IChannelMessageRepository ChannelMessages { get; }
    IChannelUserRepository ChannelUsers { get; }
    IDepartmentRepository Departments { get; }
    IDepartmentUserRepository DepartmentUsers { get; }
    IFileRepository Files { get; }
    INotificationRepository Notifications { get; }
    IRoleRepository Roles { get; }
    ITaskCommentRepository TaskComments { get; }
    ITaskDepartmentRepository TaskDepartments { get; }
    ITaskRepository Tasks { get; }
    ITaskHistoryRepository TaskHistories { get; }
    ITaskUserRepository TaskUsers { get; }
    IUserMessageRepository UserMessages { get; }
    IUserRepository Users { get; }
    IResourceRepository Resources { get; }
    int Save();
}