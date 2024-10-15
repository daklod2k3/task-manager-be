namespace server.Interfaces
{
    public interface IUnitOfWork
    {
        ITaskRepository Task {  get; }
        void Save();
    }
}
