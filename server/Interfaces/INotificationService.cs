using server.Entities;
using System.Linq.Expressions;

namespace server.Interfaces;

public interface INotificationService
{
    IEnumerable<Notification> GetAllNotifications();

    Notification CreateNotification(Notification notification);

    Notification DeleteNotification(long id);
    Notification UpdateNotification(Notification notification);

    public IEnumerable<Notification> GetNotificationById(Guid id, Expression<Func<Notification, bool>>? compositeFilterExpression);
    public IEnumerable<Notification> GetNotificationByFilter(Expression<Func<Notification, bool>>? compositeFilterExpression);



}