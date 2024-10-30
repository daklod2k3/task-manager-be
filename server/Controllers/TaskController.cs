using System.Net;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

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
    public ActionResult CreateTask(ETask eTask)
    {
        try
        {
            return Ok(_taskService.CreatTask(eTask));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Task is not create" });
        }
    }

    [HttpPut]
    public ActionResult UpdateTask(ETask eTask)
    {
        try
        {
            return Ok(_taskService.UpdateTask(eTask));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Task is not update" });
        }
    }

    [HttpDelete]
    public ActionResult DeleteTask(long id)
    {
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
    public ActionResult<IEnumerable<ETask>> GetTaskGetTaskByIdUser()
    {
        var id = HttpContext.Items["user_id"] as string;
        if (id == null)
            return BadRequest(new ErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Message = "User id error"
            });

        var taskList = _taskService.GetTaskByIdUser(new Guid(id));
        return Ok(new SuccessResponse<ETask> { Data = taskList });
    }
}