using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class TaskRepository : Repository<ETask>, ITaskRepository
{
    private readonly SupabaseContext _context;

    public TaskRepository(SupabaseContext context) : base(context)
    {
    }


    public ETask Update(ETask eTask)
    {
        return _context.Tasks.Update(eTask).Entity;
    }
}