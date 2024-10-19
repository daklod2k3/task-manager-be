using server.Entities;

namespace server.Interfaces
{
    public interface ITaskService
    {
        IEnumerable<Tasks> GetAllTask();
        Tasks CreatTask(Tasks task);
        int AssignTaskToDepartment(TaskDepartment[] taskDepartments);
        int AssignTaskToUser(TaskUser[] taskUsers);
    }
}
