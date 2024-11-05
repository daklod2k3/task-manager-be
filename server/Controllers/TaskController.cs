using System.Linq.Expressions;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
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

    public ActionResult<IEnumerable<ETask>> GetTaskByIdUser(string userId, string filterString)
    {
        

        var filterResult = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString))
        {
            filterResult = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        }
        var filter = CompositeFilter<ETask>.ApplyFilter(filterResult);
        var taskList = _taskService.GetTaskByIdUser(new Guid(userId),filter);
        return Ok(new SuccessResponse<ETask> { Data = taskList });
    }
    [HttpGet]
    public ActionResult<IEnumerable<ETask>> Get(string filter)
    {
        string id = HttpContext.Items["user_id"] as string;
        return GetTaskByIdUser(id, filter);
    }
    [HttpGet]
    [Route("{userId}")]
    public ActionResult<IEnumerable<ETask>> GetById(string userId, string filter)
    {
        return GetTaskByIdUser(userId, filter);
    }
    public ActionResult<IEnumerable<ETask>> GetTaskByFilter(string filterString)
    {
        var filterResult = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString))
        {
            filterResult = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        }
        var compositeFilterExpression = CompositeFilter<ETask>.ApplyFilter(filterResult);

        var taskList = _taskService.GetTaskByFilter(compositeFilterExpression);
        return Ok(new SuccessResponse<ETask> { Data = taskList });
    }

}