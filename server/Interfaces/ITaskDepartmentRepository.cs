using server.Entities;

namespace server.Interfaces
{
    public interface ITaskDepartmentRepository : IRepository<TaskDepartment>
    {
        public TaskDepartment Update(TaskDepartment taskDepartment);
    }
}
