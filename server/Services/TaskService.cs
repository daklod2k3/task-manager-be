using Microsoft.AspNetCore.Mvc;
using server.Context;
using server.Entities;

namespace server.Services;

public class TaskService
{
    SupabaseContext _context;

    public ActionResult<IEnumerable<Tasks>> getTask()
    {
        return _context.Tasks.ToList();
    }
}