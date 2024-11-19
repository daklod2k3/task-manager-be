using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskDepartmentController : Controller
    {
        private readonly ITaskService _taskService;
        
        public TaskDepartmentController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        [HttpPost]
        public IActionResult AssignTaskToDepartment(TaskDepartment taskDepartment)
        {

            try
            {
                return new SuccessResponse<TaskDepartment>(_taskService.AssignTaskToDepartment(taskDepartment));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ErrorResponse("Task is not assign to department");
            }
        }
        [HttpPut]
        public ActionResult UpdateAssignTaskToDepartment(TaskDepartment taskDepartment)
        {
            try
            {
                return new SuccessResponse<TaskDepartment>(_taskService.UpdateAssignTaskToDepartment(taskDepartment));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ErrorResponse("TaskDepartment is not update");
            }
        }
        [HttpDelete]
        public ActionResult DeleteAssignTaskToDepartment(long id)
        {
            try
            {
                return new SuccessResponse<TaskDepartment>(_taskService.DeleteAssignTaskToDepartment(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ErrorResponse("TaskDepartment is not delete");
            }
            
        }
    }
}
