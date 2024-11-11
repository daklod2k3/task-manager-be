namespace server.Interfaces;
using server.Entities;

public interface IUnitNotification{
    IRepository<Notification> Notification { get; }
    int Save();
}