using server.Entities;

namespace server.Interfaces
{
    public interface ITaskService
    {
        IEnumerable<Tasks> GetAllTask();
        Tasks CreatTask(Tasks task);
        int AssignTaskToDepartment(TaskDepartment[] taskDepartments);
        int AssignTaskToUser(TaskUser[] taskUsers);
        Tasks DeleteTask(long idTask);
        Tasks UpdateTask(Tasks task);
        TaskDepartment UpdateAssignTaskToDepartment(TaskDepartment taskDepartment);
        TaskDepartment DeleteAssignTaskToDepartment(long id);
        TaskUser UpdateAssignTaskToUser(TaskUser taskUser);
        TaskUser DeleteAssignTaskToUser(long id);

    }
}
