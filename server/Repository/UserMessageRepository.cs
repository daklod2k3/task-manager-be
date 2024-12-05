using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class UserMessageRepository : Repository<UserMessage>, IUserMessageRepository
{
    public UserMessageRepository(SupabaseContext context) : base(context)
    {
    }
}