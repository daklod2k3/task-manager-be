using server.Context;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Repository;

public class UserMessageRepository : Repository<UserMessage>, IUserMessageRepository
{
    public UserMessageRepository(SupabaseContext context) : base(context)
    {
    }

    public IQueryable<UserMessage> GetUserMessages(Guid userId)
    {
        return dbSet.Where(m => m.FromId == userId || m.ToId == userId);
    }

    public IQueryable<UserMessage> GetUserMessageList(Guid userId, IQueryable<UserMessage> source = null)
    {
        var query = source ?? dbSet;

        return query.GetInclude("From,To")
            .Where(m => m.FromId == userId || m.ToId == userId)
            .GroupBy(m => new
            {
                MinId = m.FromId < m.ToId ? m.FromId : m.ToId,
                MaxId = m.FromId > m.ToId ? m.ToId : m.FromId
            }).Select(g => g.OrderBy(m => m.CreatedAt)
                .First());
    }

    public IQueryable<UserMessage> GetDirectMessages(Guid userId, Guid toId, IQueryable<UserMessage> source = null)
    {
        var query = source ?? dbSet;
        return query.GetInclude("From.Role,To.Role")
            .Where(m => (m.FromId == userId && m.ToId == toId) || (m.ToId == userId && m.FromId == toId))
            .GetOrderBy();
    }
}