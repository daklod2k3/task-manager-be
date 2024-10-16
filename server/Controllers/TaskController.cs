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

    [HttpGet]
    public ActionResult<IEnumerable<Tasks>> GetAllTask()
    {

        var taskList = _taskService.GetAllTask();
        return Ok(new { taskList });
    }
}