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
    private readonly IUnitOfWork _unitOfWork;

    public TaskController(ITaskService taskService, IUnitOfWork unitOfWork)
    {
        _taskService = taskService;
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    public ActionResult CreateTask(TaskEntity taskEntity)
    {
        var id = AuthController.GetUserId(HttpContext);
        taskEntity.CreatedBy = new Guid(id);
        try
        {
            return new SuccessResponse<TaskEntity>(_taskService.CreatTask(taskEntity));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Task is not create");
        }
    }

    [HttpPut]
    public ActionResult UpdateTask(TaskEntity taskEntity)
    {
        try
        {
            return new SuccessResponse<TaskEntity>(_taskService.UpdateTask(taskEntity));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Task is not update");
        }
    }

    [HttpPatch("{id}")]
    public ActionResult UpdateTask(long id, [FromBody] JsonPatchDocument<TaskEntity> patchDoc)
    {
        try
        {
            return new SuccessResponse<TaskEntity>(_taskService.UpdateTask(id, patchDoc));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Task is not update");
        }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteTask(long id)
    {
        try
        {
            return new SuccessResponse<TaskEntity>(_taskService.DeleteTask(id));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Task is not delete");
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<IEnumerable<TaskEntity>> GetTaskByIdUser(string userId, string? filterString,
        string? includeProperties, int? page, int? pageItem)
    {
        Pagination pagination = null;
        if (page != null && pageItem != null)
            pagination = new Pagination { PageNumber = (int)page, PageSize = (int)pageItem };
        var filterResult = new ClientFilter();
        Expression<Func<TaskEntity, bool>>? filter = null;

        if (!string.IsNullOrEmpty(filterString))
        {
            filterResult = JsonConvert.DeserializeObject<ClientFilter>(filterString);
            filter = CompositeFilter<TaskEntity>.ApplyFilter(filterResult);
        }

        var taskList = _taskService.GetTaskByIdUser(new Guid(userId), filter, includeProperties, pagination);
        return new SuccessResponse<IEnumerable<TaskEntity>>(taskList);
    }

    [HttpGet]
    public ActionResult<IEnumerable<TaskEntity>> Get(string? filter, string? includes, int? page, int? pageItem)
    {
        var id = AuthController.GetUserId(HttpContext);
        return GetTaskByIdUser(id, filter, includes, page, pageItem);
    }

    [HttpGet]
    [Route("{taskId}")]
    public ActionResult<IEnumerable<TaskEntity>> GetTaskById(long taskId, string? includes)
    {
        var id = AuthController.GetUserId(HttpContext);
        try
        {
            return new SuccessResponse<TaskEntity>(_taskService.GetTask(new Guid(id), taskId, includes));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Task is not found");
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<IEnumerable<TaskEntity>> GetTaskByFilter(string filterString)
    {
        var filterResult = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString))
            filterResult = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        var compositeFilterExpression = CompositeFilter<TaskEntity>.ApplyFilter(filterResult);

        var taskList = _taskService.GetTaskByFilter(compositeFilterExpression);
        return new SuccessResponse<IEnumerable<TaskEntity>>(taskList);
    }

    [HttpGet("replace/{property}/{value}")]
    public IActionResult ReplaceProperty([FromQuery] long id, [FromRoute] string property, [FromRoute] string value)
    {
        var jsonPatchString = "[{\"op\":\"replace\",\"path\":\"" + property + "\",\"value\":\"" + value + "\"}]";
        var json = JsonConvert.DeserializeObject<JsonPatchDocument<TaskEntity>>(jsonPatchString);
        var task = _unitOfWork.Tasks.GetById(id);
        json.ApplyTo(task);
        _unitOfWork.Save();
        return new SuccessResponse<TaskEntity>(task);
    }
}