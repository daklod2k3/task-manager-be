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
    public Notification GetNotificationById(Guid id);
    public IEnumerable<Notification> GetNotificationByFilter(Expression<Func<Notification, bool>> compositeFilterExpression, string? includeProperties);



}