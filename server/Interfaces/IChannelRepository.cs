using System.Linq.Expressions;
using server.Entities;

namespace server.Interfaces;

public interface IChannelRepository : IRepository<Channel>
{
    public IEnumerable<Channel> GetByUser(Guid id, Expression<Func<Channel, bool>>? filter = null,
        string? includeProperties = null,
        string? orderBy = null,
        int? page = null, int? pageSize = null);
}