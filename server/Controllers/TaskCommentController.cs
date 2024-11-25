using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskCommentController : Controller
{
    private readonly IRepository<TaskComment> _repository;

    public TaskCommentController(IRepository<TaskComment> taskCommentRepository)
    {
        _repository = taskCommentRepository;
    }

    [HttpPost]
    public ActionResult Create(TaskComment comment)
    {
        var id = AuthController.GetUserId(HttpContext);
        comment.CreatedBy = new Guid(id);
        try
        {
            var entity = _repository.Add(comment);
            _repository.Save();
            return new SuccessResponse<TaskComment>(entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpPut]
    public ActionResult Update(TaskComment body)
    {
        try
        {
            var comment = _repository.Update(body);
            _repository.Save();
            return new SuccessResponse<TaskComment>(comment);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<TaskComment> patchDoc)
    {
        try
        {
            return new SuccessResponse<TaskComment>(_repository.UpdatePatch(id.ToString(), patchDoc));
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
            return new SuccessResponse<TaskComment>(null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpDelete]
    public ActionResult Delete(TaskComment body)
    {
        try
        {
            _repository.Remove(body);
            _repository.Save();
            return new SuccessResponse<TaskComment>(body);
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
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<TaskComment>>(
            _repository.Get(CompositeFilter<TaskComment>.ApplyFilter(filter), includeProperties: includes));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<IEnumerable<TaskComment>> GetId(long id, string? includes = "")
    {
        try
        {
            return new SuccessResponse<TaskComment>(_repository.GetById(id.ToString(), includes));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }
}