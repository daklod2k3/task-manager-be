using Microsoft.EntityFrameworkCore;
using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class UserRepository : Repository<Profile>, IUserRepository
{
    private readonly DbSet<Profile> _dbSet;

    public UserRepository(SupabaseContext context) : base(context)
    {
        _dbSet = context.Set<Profile>();
    }

    public Profile GetById(Guid id)
    {
        if (id != Guid.Empty) return _dbSet.Find(id);
        return null;
    }
}