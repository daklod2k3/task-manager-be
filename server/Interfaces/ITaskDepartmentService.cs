using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using System.Linq.Expressions;

namespace server.Interfaces
{
    public interface ITaskDepartmentService
    {
        IEnumerable<TaskDepartment> GetAllTaskDepartment();
        TaskDepartment CreateTaskDepartment(TaskDepartment taskDepartment);
        TaskDepartment GetTaskDepartmentById(long id);
        TaskDepartment UpdateTaskDepartmentById(long id, TaskDepartment taskDepartment);
        TaskDepartment PatchTaskDepartmentById(long id, [FromBody] JsonPatchDocument<TaskDepartment> patchDoc);
        TaskDepartment DeleteTaskDepartmentById(long id);
    }
}
