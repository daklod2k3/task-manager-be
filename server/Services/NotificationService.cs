using System.Linq.Expressions;
using LinqKit;
using server.Entities;
using server.Interfaces;

namespace server.Services;

public class NotificationService : INotificationService
{
    private readonly IUnitNotification _unitnotification;

    public NotificationService(IUnitNotification unitnotification)
    {
        _unitnotification = unitnotification;
    }

    public Notification CreateNotification(Notification notification)
    {
        var result = _unitnotification.Notification.Add(notification);
        _unitnotification.Save();
        return result;
    }

    public IEnumerable<Notification> GetAllNotifications()
    {
        return _unitnotification.Notification.GetAll();
    }

    public Notification DeleteNotification(long id)
    {
        var notification = _unitnotification.Notification.Get(x => x.Id == id);
        var result = _unitnotification.Notification.Remove(notification);
        _unitnotification.Save();
        return result;
    }

    public Notification UpdateNotification(Notification notification)
    {
        var result = _unitnotification.Notification.Update(notification);
        return result;
    }

    public IEnumerable<Notification> GetNotificationById(Guid id, Expression<Func<Notification, bool>>? filter)
    {
        if (Guid.Empty == id) return Enumerable.Empty<Notification>();
        var result = _unitnotification.Notification.GetAll(filter.And(t => t.UserId == id)).ToList();
        return result;
    }

    public IEnumerable<Notification> GetNotificationByFilter(Expression<Func<Notification, bool>> filter)
    {
        return _unitnotification.Notification.GetAll(filter);
    }

    
}