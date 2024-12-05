using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class ChannelUserRepository : Repository<ChannelUser>, IChannelUserRepository
{
    public ChannelUserRepository(SupabaseContext context) : base(context)
    {
    }
}