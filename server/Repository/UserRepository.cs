using Microsoft.EntityFrameworkCore;
using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class UserRepository : Repository<Profile>, IUserRepository
{
    private readonly DbSet<Profile> dbSet;

    public UserRepository(SupabaseContext context) : base(context)
    {
    }

    public Profile GetById(Guid id)
    {
        return dbSet.Find(id);
    }
}