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
       
    }
}
