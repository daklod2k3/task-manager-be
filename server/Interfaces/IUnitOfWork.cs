namespace server.Interfaces;

public interface IUnitOfWork
{
    ITaskRepository Task { get; }
    ITaskDepartmentRepository TaskDepartment { get; }
    IDepartment Department { get; }
    IDepartmentUser DepartmentUser { get; }
    ITaskUserRepository TaskUser { get; }
    IUserRepository User { get; }
    IRoleRepository Role { get; }
    IFileRepository FileRepository { get; }
    int Save();
}