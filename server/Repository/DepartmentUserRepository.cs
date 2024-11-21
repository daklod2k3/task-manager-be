using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository
{
    public class DepartmentUserRepository : Repository<DepartmentUser>, IDepartmentUser
    {
        public DepartmentUserRepository(SupabaseContext context) : base(context)
        {
        }
    }
}
