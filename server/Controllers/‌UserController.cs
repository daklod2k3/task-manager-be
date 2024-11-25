using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Context;
using server.Entities;
using server.Helpers;
using server.Interfaces;
using server.Services;
using System.Linq.Expressions;
using Client = Supabase.Client;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly Client _client;

    private readonly SupabaseContext _context;
    private readonly UserService _userService;
    private readonly IUnitOfWork _unitOfWork;

    public UserController(SupabaseContext context, Client client, UserService userService, IUnitOfWork unitOfWork)
    {
        _context = context;
        _client = client;
        _userService = userService;
        _unitOfWork = unitOfWork;
    }

    // GET
    [HttpGet]
    public ActionResult GetUserAuthorized()
    {
        var user_id = AuthController.GetUserId(HttpContext);

        return GetProfile(user_id);
    }

    [HttpGet("{user_id}")]
    public ActionResult GetProfile(string user_id)
    {
        if (user_id == null) 
            return new ErrorResponse("no user found");
        var user = _userService.GetProfile(new Guid(user_id));
        return new SuccessResponse<Profile>( user );
    }
    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<IEnumerable<TaskEntity>> GetTaskByIdUser(string userId, string? filterString,string? orderBy,
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

        var taskList = _unitOfWork.Task.GetTaskByIdUser(new Guid(userId), filter, orderBy, includeProperties, pagination);
        return new SuccessResponse<IEnumerable<TaskEntity>>(taskList);
    }
    [HttpGet("task")]
    public ActionResult<IEnumerable<TaskEntity>> GetTask(string? filter,string? orderBy, string? includes, int? page, int? pageItem)
    {
        var user_id = AuthController.GetUserId(HttpContext);
        return GetTaskByIdUser(user_id, filter, orderBy, includes, page, pageItem);
    }
}