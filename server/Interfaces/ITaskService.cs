using server.Entities;

namespace server.Interfaces
{
    public interface ITaskService
    {
        IEnumerable<Tasks> GetAllTask();
        void CreatTask(Tasks task);
    }
}
