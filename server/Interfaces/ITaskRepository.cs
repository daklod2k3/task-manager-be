using server.Entities;

namespace server.Interfaces
{
    public interface ITaskRepository : IRepository<Tasks>
    {
        public void Update (Tasks task);
    }
}
