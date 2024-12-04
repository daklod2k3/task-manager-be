using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class ResourceRepository : Repository<Resource>, IResourceRepository
{
    public ResourceRepository(SupabaseContext context) : base(context)
    {

    }
}