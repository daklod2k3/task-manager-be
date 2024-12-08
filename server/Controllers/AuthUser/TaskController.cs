using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Controllers.AuthUser;

[ApiController]
[Route("auth/user/[controller]")]
public class TaskController : Controller
{
    private readonly IRepository<TaskEntity> _repository;

    public TaskController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.Tasks;
    }

    [HttpPost]
    public ActionResult Create(TaskEntity body)
    {
        var id = AuthController.GetUserId(HttpContext);
        body.CreatedBy = new Guid(id);
        try
        {
            var entity = _repository.Add(body);
            _repository.Save();
            return new SuccessResponse<TaskEntity>(entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpPut]
    public ActionResult Update(TaskEntity body)
    {
        try
        {
            var entity = _repository.Update(body);
            _repository.Save();
            return new SuccessResponse<TaskEntity>(entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<TaskEntity> patchDoc)
    {
        return new SuccessResponse<TaskEntity>(_repository.UpdatePatch(id, patchDoc));
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
    public ActionResult Delete(TaskEntity body)
    {
        try
        {
            _repository.Remove(body);
            _repository.Save();
            return new SuccessResponse<TaskEntity>(body);
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
        return new SuccessResponse<IEnumerable<TaskEntity>>(
            _repository.Get(CompositeFilter<TaskEntity>.ApplyFilter(filter), includes));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<IEnumerable<TaskEntity>> GetId(long id, string? includes = "")
    {
        try
        {
            return new SuccessResponse<TaskEntity>(_repository.GetById(id.ToString(), includes));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }
}