using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq.Expressions;
using server.Helpers;
namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskUserController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ITaskUserService _taskUserService;
        public TaskUserController(ITaskService taskService, ITaskUserService taskUserService)
        {
            _taskService = taskService;
            _taskUserService = taskUserService;
        }

        [HttpGet]
        public IActionResult GetAllTaskUser()
        {
            try
            {
                return Ok(_taskUserService.GetAllTaskUsers());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskUser is not found" });
            }
        }
        //phuong thuc get theo id
        [HttpGet("{id}")]
        public IActionResult GetTaskUserById(long id)
        {
            try
            {
                return Ok(_taskUserService.GetTaskUserById(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskUser is not found" });
            }
        }

        //phuong thuc post tao 1 department
        [HttpPost("create")]
        public IActionResult CreateTaskUser(TaskUser taskUser)
        {
            try
            {
                return Ok(_taskUserService.CreateTaskUser(taskUser));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskUser is not create" });
            }
        }

        //phuong thuc put update 1 department
        [HttpPut("{id}")]
        public IActionResult UpdateTaskUserById(long id, TaskUser taskUser)
        {
            try
            {
                return Ok(_taskUserService.UpdateTaskUserById(id, taskUser));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskUser is not update" });
            }
        }

        //phuong thuc patch update 1 taskuser
        [HttpPatch("{id}")]
        public IActionResult PatchTaskUserById(long id, [FromBody] JsonPatchDocument<TaskUser> patchDoc)
        {
            try
            {
                return Ok(_taskUserService.PatchTaskUserById(id, patchDoc));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskUser is not update" });
            }
        }

        //phuong thuc delete 1 taskuser
        public IActionResult DeleteTaskUserById(long id)
        {
            try
            {
                return Ok(_taskUserService.DeleteTaskUserById(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskUser is not delete" });
            }
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
