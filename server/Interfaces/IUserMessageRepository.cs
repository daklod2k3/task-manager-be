using server.Entities;

namespace server.Interfaces;

public interface IUserMessageRepository : IRepository<UserMessage>
{
    public IQueryable<UserMessage> GetUserMessages(Guid userId);
    public IQueryable<UserMessage> GetUserMessageList(Guid userId, IQueryable<UserMessage> source = null);
    public IQueryable<UserMessage> GetDirectMessages(Guid userId, Guid toId, IQueryable<UserMessage> source = null);
}