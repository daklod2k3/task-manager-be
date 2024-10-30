using server.Entities;

namespace server.Interfaces;

public interface ITaskRepository : IRepository<ETask>
{
    public ETask Update(ETask eTask);
}