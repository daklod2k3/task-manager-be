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
    private readonly IUnitOfWork _unitOfWork;

    public TaskController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Tasks>> GetAllTask()
    {
        var listTasks = _unitOfWork.Task.GetAll();
        return Ok( new { listTasks });
    }
}