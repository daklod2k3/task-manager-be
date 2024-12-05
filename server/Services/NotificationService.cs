using System.Linq.Expressions;
using LinqKit;
using server.Entities;
using server.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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
        CreateNotification(new Notification());
        return _unitnotification.Notification.Get();
    }

    public Notification DeleteNotification(long id)
    {
        var notification = _unitnotification.Notification.GetById(id);
        var result = _unitnotification.Notification.Remove(notification);
        _unitnotification.Save();
        return result;
    }

    public Notification UpdateNotification(Notification notification)
    {
        var result = _unitnotification.Notification.Update(notification);
        return result;
    }

    public Notification PatchNotification(long id,[FromBody] JsonPatchDocument<Notification> notification)
    {
        var noti = _unitnotification.Notification.GetById(id);
        if (noti == null) throw new Exception("not found notification");

        notification.ApplyTo(noti);

        _unitnotification.Save();

        return noti;
    }

    public Notification GetNotificationById(Guid id)
    {
        var result = _unitnotification.Notification.GetById(id);
        return result;
    }

    public IEnumerable<Notification> GetNotificationByFilter(Expression<Func<Notification, bool>> filter, string includeProperties)
    {
        return _unitnotification.Notification.Get(filter, null, includeProperties);
    }

    
}