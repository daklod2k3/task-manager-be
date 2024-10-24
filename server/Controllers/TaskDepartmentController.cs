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
        public IActionResult AssignTaskToDepartment(TaskDepartment[] taskDepartments)
        {
            
            return Ok(_taskService.AssignTaskToDepartment(taskDepartments));
        }
        [HttpPut]
        public ActionResult UpdateAssignTaskToDepartment(TaskDepartment taskDepartment)
        {
            try
            {
                return Ok(_taskService.UpdateAssignTaskToDepartment(taskDepartment));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskDepartment is not update" });
            }
        }
        [HttpDelete]
        public ActionResult DeleteAssignTaskToDepartment(long id)
        {
            try
            {
                return Ok(_taskService.DeleteAssignTaskToDepartment(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskDepartment is not delete" });
            }
        }
    }
}
