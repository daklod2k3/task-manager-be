using server.Entities;

namespace server.Interfaces
{
    public interface ITaskUserRepository : IRepository<TaskUser>
    {
        public TaskUser Update(TaskUser taskUser);
    }
}
