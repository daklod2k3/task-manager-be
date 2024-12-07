using System.Linq.Expressions;
using server.Context;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Repository;

public class ChannelRepository : Repository<Channel>, IChannelRepository
{
    public ChannelRepository(SupabaseContext context) : base(context)
    {
    }

    public IEnumerable<Channel> GetByUser(Guid id, Expression<Func<Channel, bool>>? filter = null,
        string? includeProperties = null,
        string? orderBy = null,
        int? page = null, int? pageSize = null)
    {
        var query = dbSet.GetInclude(includeProperties);

        return query.Where(c => c.ChannelUsers.Any(u => u.UserId == id)
                                || c.Department.DepartmentUsers.Any(u => u.UserId == id))
            .Union(GetQuery(filter, includeProperties, orderBy, page, pageSize));
    }
}