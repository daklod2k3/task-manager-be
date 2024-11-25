using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(SupabaseContext context) : base(context)
    {
    }
}