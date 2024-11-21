using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskDepartmentController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ITaskDepartmentService _taskDepartmentService;
        public TaskDepartmentController(ITaskService taskService, ITaskDepartmentService taskDepartmentService)
        {
            _taskService = taskService;
            _taskDepartmentService = taskDepartmentService;
        }

        //phuong thuc get
        [HttpGet]
        public IActionResult GetAllTaskDepartment()
        {
            try
            {
                return Ok(_taskDepartmentService.GetAllTaskDepartment());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskDepartment is not found" });
            }
        }

        //phuong thuc get theo id
        [HttpGet("{id}")]
        public IActionResult GetTaskDepartmentById(long id)
        {
            try
            {
                return Ok(_taskDepartmentService.GetTaskDepartmentById(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskDepartment is not found" });
            }
        }

        //phuong thuc post tao 1 taskdepartment
        [HttpPost("create")]
        public IActionResult CreateTaskDepartment(TaskDepartment taskDepartment)
        {
            try
            {
                return Ok(_taskDepartmentService.CreateTaskDepartment(taskDepartment));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskDepartment is not create" });
            }
        }

        //phuong thuc put update 1 taskdepartment
        [HttpPut("{id}")]
        public IActionResult UpdateTaskDepartmentById(long id, TaskDepartment taskDepartment)
        {
            try
            {
                return Ok(_taskDepartmentService.UpdateTaskDepartmentById(id, taskDepartment));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskDepartment is not update" });
            }
        }

        //phuong thuc patch update 1 taskdepartment
        [HttpPatch("{id}")]
        public IActionResult UpdateTaskDepartmentById(long id, [FromBody] JsonPatchDocument<TaskDepartment> taskDepartment)
        {
            try
            {
                var taskDepartmentEntity = _taskDepartmentService.GetTaskDepartmentById(id);
                taskDepartment.ApplyTo(taskDepartmentEntity, ModelState);
                return Ok(_taskDepartmentService.UpdateTaskDepartmentById(id, taskDepartmentEntity));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskDepartment is not update" });
            }
        }

        //phuong thuc delete 1 taskdepartment
        [HttpDelete("{id}")]
        public IActionResult DeleteTaskDepartmentById(long id)
        {
            try
            {
                return Ok(_taskDepartmentService.DeleteTaskDepartmentById(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskDepartment is not delete" });
            }
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
