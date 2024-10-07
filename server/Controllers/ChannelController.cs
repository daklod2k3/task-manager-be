using Microsoft.AspNetCore.Mvc;
using server.Context;
using server.Entities;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class ChannelController : Controller
{
    private readonly SupabaseContext _context;

    public ChannelController(SupabaseContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Channel>> Get()
    {
        return _context.Channels.ToList();
    }
}