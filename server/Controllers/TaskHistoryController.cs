using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskHistoryController : Controller
{
    private readonly IRepository<TaskHistory> _repository;

    public TaskHistoryController(IRepository<TaskHistory> taskHistoryRepository)
    {
        _repository = taskHistoryRepository;
    }

    [HttpPost]
    public ActionResult Create(TaskHistory comment)
    {
        var id = AuthController.GetUserId(HttpContext);
        comment.CreatedBy = new Guid(id);
        try
        {
            var entity = _repository.Add(comment);
            _repository.Save();
            return new SuccessResponse<TaskHistory>(entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpPut]
    public ActionResult Update(TaskHistory body)
    {
        try
        {
            var comment = _repository.Update(body);
            _repository.Save();
            return new SuccessResponse<TaskHistory>(comment);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<TaskHistory> patchDoc)
    {
        return new SuccessResponse<TaskHistory>(_repository.UpdatePatch(id.ToString(), patchDoc));
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteId(long id)
    {
        var entity = _repository.GetById(id.ToString());
        _repository.Remove(entity);
        _repository.Save();
        return new SuccessResponse<TaskHistory>(null);
    }

    [HttpDelete]
    public ActionResult Delete(TaskHistory body)
    {
        try
        {
            _repository.Remove(body);
            _repository.Save();
            return new SuccessResponse<TaskHistory>(body);
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
        return new SuccessResponse<IEnumerable<TaskHistory>>(
            _repository.Get(CompositeFilter<TaskHistory>.ApplyFilter(filter), includeProperties: includes));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<IEnumerable<TaskHistory>> GetId(long id, string? includes = "")
    {
        try
        {
            return new SuccessResponse<TaskHistory>(_repository.GetById(id.ToString(), includes));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }
}