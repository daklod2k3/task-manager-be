using Microsoft.AspNetCore.Mvc;
using server.Context;
using server.Entities;
using server.Services;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : Controller
{
    SupabaseContext _context;

    public TaskController(SupabaseContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<Tasks>> Get()
    {
        return _context.Tasks.ToList();
    }
}