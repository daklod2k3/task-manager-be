using Microsoft.AspNetCore.Mvc;
using server.Context;
using server.Entities;
using server.Interfaces;
using server.Repository;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : Controller
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    
    [HttpPost]
    public ActionResult CreateTask(Tasks task)
    {
        try {
            return Ok(_taskService.CreatTask(task));
        }
        catch (Exception ex) {
            Console.WriteLine(ex.ToString());
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Task is not create" });
        }
    }
    [HttpPut]
    public ActionResult UpdateTask(Tasks task) {
        try
        {
            return Ok(_taskService.UpdateTask(task));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Task is not update" });
        }
    }
    [HttpDelete]
    public ActionResult DeleteTask(long id) {
        try
        {
            return Ok(_taskService.DeleteTask(id));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Task is not delete" });
        }
    }
    [HttpGet]
    public ActionResult<IEnumerable<Tasks>> GetTaskGetTaskByIdUser(Guid id)
    {

        var taskList = _taskService.GetTaskByIdUser(id);
        return Ok(new { taskList });
    }
}