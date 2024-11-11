using Microsoft.EntityFrameworkCore.ChangeTracking;
using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Repository
{
    public class TaskDepartmentRepository : Repository<TaskDepartment>, ITaskDepartmentRepository
    {
        private readonly SupabaseContext _context;
        public TaskDepartmentRepository(SupabaseContext context) : base(context)
        {
            _context = context;

        }
        public TaskDepartment Update(TaskDepartment taskDepartment)
        {
            return _context.TaskDepartments.Update(taskDepartment).Entity;
        }


    }
}
