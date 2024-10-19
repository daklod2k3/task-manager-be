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
    }
}
