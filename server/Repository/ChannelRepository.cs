using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class ChannelRepository : Repository<Channel>, IChannelRepository
{
    public ChannelRepository(SupabaseContext context) : base(context)
    {
    }
}