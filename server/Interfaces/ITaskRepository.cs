using server.Entities;

namespace server.Interfaces
{
    public interface ITaskRepository : IRepository<Tasks>
    {
        public Tasks Update (Tasks task);
    }
}
