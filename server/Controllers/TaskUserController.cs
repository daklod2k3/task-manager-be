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
        public IActionResult AssignTaskToUser(TaskUser taskUser)
        {
            try
            {
                return new SuccessResponse<TaskUser>(_taskService.AssignTaskToUser(taskUser));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ErrorResponse("Task is not assign to user");
            }

        }
        [HttpPut]
        public ActionResult UpdateAssignTaskToUser(TaskUser taskUser)
        {
            try
            {
                return new SuccessResponse<TaskUser>(_taskService.UpdateAssignTaskToUser(taskUser));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ErrorResponse("TaskUser is not update");
            }
        }
        [HttpDelete]
        public ActionResult DeleteAssignTaskToUser(long id)
        {
            try
            {
                return new SuccessResponse<TaskUser>(_taskService.DeleteAssignTaskToUser(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ErrorResponse("TaskUser is not delete");
            }
        }
    }
}
