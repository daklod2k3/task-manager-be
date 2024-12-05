using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository;

public class NotificationRepository : Repository<Notification>, INotificationRepository
{
    public NotificationRepository(SupabaseContext context) : base(context)
    {
    }
}