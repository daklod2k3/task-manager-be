using System.Linq.Expressions;
using server.Entities;

namespace server.Interfaces;

public interface ITaskRepository : IRepository<ETask>
{
    // Phương thức lấy tất cả task
    Task<List<ETask>> GetAllAsync();

    // Phương thức tìm task theo điều kiện
    Task<List<ETask>> GetAsync(Expression<Func<ETask, bool>> predicate);

}