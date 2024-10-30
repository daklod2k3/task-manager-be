namespace server.Interfaces;

public interface IUnitOfWork
{
    ITaskRepository Task { get; }
    ITaskDepartmentRepository TaskDepartment { get; }
    ITaskUserRepository TaskUser { get; }
    IUserRepository User { get; }
    int Save();
}