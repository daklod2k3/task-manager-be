using System.Linq.Expressions;
using LinqKit;
using server.Entities;
using server.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace server.Services;

public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitofwork;

    public NotificationService(IUnitOfWork unitofwork)
    {
        _unitofwork = unitofwork;
    }

    public Notification CreateNotification(Notification notification)
    {
        var result = _unitofwork.Notifications.Add(notification);
        _unitofwork.Save();
        return result;
    }

    public IEnumerable<Notification> GetAllNotifications()
    {
        CreateNotification(new Notification());
        return _unitofwork.Notifications.Get();
    }

    public Notification DeleteNotification(long id)
    {
        var notification = _unitofwork.Notifications.GetById(id);
        var result = _unitofwork.Notifications.Remove(notification);
        _unitofwork.Save();
        return result;
    }

    public Notification UpdateNotification(Notification notification)
    {
        var result = _unitofwork.Notifications.Update(notification);
        return result;
    }

    public Notification PatchNotification(long id,[FromBody] JsonPatchDocument<Notification> notification)
    {
        var noti = _unitofwork.Notifications.GetById(id);
        if (noti == null) throw new Exception("not found notification");

        notification.ApplyTo(noti);

        _unitofwork.Save();

        return noti;
    }

    public Notification GetNotificationById(Guid id)
    {
        var result = _unitofwork.Notifications.GetById(id);
        return result;
    }

    public IEnumerable<Notification> GetNotificationByFilter(Expression<Func<Notification, bool>> filter, string includeProperties)
    {
        return _unitofwork.Notifications.Get(filter, null, includeProperties);
    }

    
}