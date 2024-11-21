using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class DepartmentUserRepository : Repository<DepartmentUser>, IDepartmentUserRepository
{
    public DepartmentUserRepository(SupabaseContext context) : base(context)
    {
    }
}