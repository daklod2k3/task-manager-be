using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class ChannelMessageRepository : Repository<ChannelMessage>, IChannelMessageRepository
{
    public ChannelMessageRepository(SupabaseContext context) : base(context)
    {
    }
}