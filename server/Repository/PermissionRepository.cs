using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class PermissionRepository : Repository<Permission>, IPermissionRepository
{
    public PermissionRepository(SupabaseContext context) : base(context)
    {

    }
}