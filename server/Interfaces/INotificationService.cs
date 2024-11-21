using server.Entities;
using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
namespace server.Interfaces;
using Microsoft.AspNetCore.Mvc;

public interface INotificationService
{
    IEnumerable<Notification> GetAllNotifications();

    Notification CreateNotification(Notification notification);

    Notification DeleteNotification(long id);
    Notification UpdateNotification(Notification notification);
    Notification PatchNotification(long id, [FromBody] JsonPatchDocument<Notification> notification);

    public IEnumerable<Notification> GetNotificationById(Guid id, Expression<Func<Notification, bool>>? compositeFilterExpression);
    public IEnumerable<Notification> GetNotificationByFilter(Expression<Func<Notification, bool>>? compositeFilterExpression);



}