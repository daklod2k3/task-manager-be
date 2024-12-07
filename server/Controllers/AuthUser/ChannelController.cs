using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Controllers.AuthUser;

[ApiController]
[Route("auth/user/[controller]")]
public class ChannelController : Controller
{
    private readonly IChannelRepository _repository;

    public ChannelController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.Channels;
    }

    [HttpPost]
    public ActionResult Create(Channel comment)
    {
        var id = AuthController.GetUserId(HttpContext);
        comment.CreatedBy = new Guid(id);
        try
        {
            var entity = _repository.Add(comment);
            _repository.Save();
            return new SuccessResponse<Channel>(entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpPut]
    public ActionResult Update(Channel body)
    {
        try
        {
            var comment = _repository.Update(body);
            _repository.Save();
            return new SuccessResponse<Channel>(comment);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<Channel> patchDoc)
    {
        try
        {
            return new SuccessResponse<Channel>(_repository.UpdatePatch(id.ToString(), patchDoc));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Task is not update");
        }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteId(long id)
    {
        try
        {
            var entity = _repository.GetById(id.ToString());
            _repository.Remove(entity);
            _repository.Save();
            return new SuccessResponse<Channel>(null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpDelete]
    public ActionResult Delete(Channel body)
    {
        try
        {
            _repository.Remove(body);
            _repository.Save();
            return new SuccessResponse<Channel>(body);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }


    [HttpGet]
    public ActionResult<IEnumerable<TaskEntity>> Get([FromQuery(Name = "filter")] string? filterString, int? page,
        int? pageItem, string? includes = "")
    {
        var user_id = new Guid(AuthController.GetUserId(HttpContext));
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<Channel>>(
            _repository.GetByUser(user_id, CompositeFilter<Channel>.ApplyFilter(filter), includes));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<IEnumerable<TaskComment>> GetId(long id, string? includes = "")
    {
        try
        {
            return new SuccessResponse<Channel>(_repository.GetById(id.ToString(), includes));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }
}