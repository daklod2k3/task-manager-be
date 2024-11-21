using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class DepartmentRepository : Repository<Department>, IDepartmentRepository
{
    public DepartmentRepository(SupabaseContext context) : base(context)
    {
    }
}