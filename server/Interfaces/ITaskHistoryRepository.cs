using server.Entities;

namespace server.Interfaces
{
    public interface ITaskHistoryRepository : IRepository<TaskHistory>
    {
        public TaskHistory Update(TaskHistory taskHistory);
    }
}