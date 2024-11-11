using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskUserController : Controller
    {
        private readonly ITaskService _taskService;
        public TaskUserController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        [HttpPost]
        public IActionResult AssignTaskToUser(TaskUser[] taskUsers)
        {

            return Ok(_taskService.AssignTaskToUser(taskUsers));
        }
        [HttpPut]
        public ActionResult UpdateAssignTaskToUser(TaskUser taskUser)
        {
            try
            {
                return Ok(_taskService.UpdateAssignTaskToUser(taskUser));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskUser is not update" });
            }
        }
        [HttpDelete]
        public ActionResult DeleteAssignTaskToUser(long id)
        {
            try
            {
                return Ok(_taskService.DeleteAssignTaskToUser(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskUser is not delete" });
            }
        }
    }
}
