namespace server.Interfaces;


public interface IUnitNotification{
    INotificationRepository Notification { get; }
    int Save();
}