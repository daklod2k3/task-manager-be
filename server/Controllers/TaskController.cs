using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
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
            return new SuccessResponse<ETask>(_taskService.CreatTask(eTask));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Task is not create");
        }
    }

    [HttpPatch("{id}")]
    public ActionResult UpdateTask(long id, [FromBody] JsonPatchDocument<ETask> patchDoc)
    {
        try
        {
            return new SuccessResponse<ETask>(_taskService.UpdateTask(id, patchDoc));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Task is not update");
        }
    }

    [HttpDelete]
    public ActionResult DeleteTask(long id)
    {
        try
        {
            return new SuccessResponse<ETask>(_taskService.DeleteTask(id));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Task is not delete");
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<IEnumerable<ETask>> GetTaskByIdUser(string userId, string? filterString)
    {
        var filterResult = new ClientFilter();
        Expression<Func<ETask, bool>>? filter = null;

        if (!string.IsNullOrEmpty(filterString))
        {
            filterResult = JsonConvert.DeserializeObject<ClientFilter>(filterString);
            filter = CompositeFilter<ETask>.ApplyFilter(filterResult);
        }

        var taskList = _taskService.GetTaskByIdUser(new Guid(userId), filter);
        return new SuccessResponse<IEnumerable<ETask>>(taskList);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ETask>> Get(string? filter)
    {
        var id = AuthController.GetUserId(HttpContext);
        return GetTaskByIdUser(id, filter);
    }

    [HttpGet]
    [Route("{taskId}")]
    public ActionResult<IEnumerable<ETask>> GetTaskById(long taskId)
    {
        try
        {
            var task = _taskService.GetTask(taskId);
            if(task == null)
            {
                return new ErrorResponse("Task is not exist");
            }
            return new SuccessResponse<ETask>(task);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Task is not get");
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<IEnumerable<ETask>> GetTaskByFilter(string filterString)
    {
        var filterResult = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString))
            filterResult = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        var compositeFilterExpression = CompositeFilter<ETask>.ApplyFilter(filterResult);

        var taskList = _taskService.GetTaskByFilter(compositeFilterExpression);
        return new SuccessResponse<IEnumerable<ETask>>(taskList);
    }
}