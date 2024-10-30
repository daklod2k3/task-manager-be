using server.Entities;
using System.Linq.Expressions;

namespace server.Interfaces
{
    public interface ITaskRepository : IRepository<Tasks>
    {
        public Tasks Update (Tasks task);
    }
}
