using server.Entities;

namespace server.Interfaces
{
    public interface ITaskUserService
    {
        IEnumerable<TaskUser> GetAllTaskUsers();
        TaskUser? GetTaskUserById(int id);
        void CreateTaskUser(TaskUser taskUser);
        bool UpdateTaskUser(int id, TaskUser updatedTaskUser);
        bool DeleteTaskUser(int id);
    }
}
