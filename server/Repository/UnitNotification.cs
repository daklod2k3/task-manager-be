using server.Context;
using server.Interfaces;

namespace server.Repository;

public class UnitNotification : IUnitNotification
{
    private readonly SupabaseContext _context;

    public UnitNotification(SupabaseContext context)
    {
        _context = context;
        Notification = new NotificationRepository(_context);
    }

    public INotificationRepository Notification { get; }

    public int Save()
    {
        return _context.SaveChanges();
    }
}