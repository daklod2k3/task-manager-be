using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskUserController : Controller
{
    private readonly IRepository<TaskUser> _repository;

    public TaskUserController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.TaskUsers;
    }

    [HttpPost]
    public ActionResult Create(TaskUser comment)
    {
        try
        {
            var entity = _repository.Add(comment);
            _repository.Save();
            return new SuccessResponse<TaskUser>(entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpPut]
    public ActionResult Update(TaskUser body)
    {
        try
        {
            var comment = _repository.Update(body);
            _repository.Save();
            return new SuccessResponse<TaskUser>(comment);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<TaskUser> patchDoc)
    {
        try
        {
            return new SuccessResponse<TaskUser>(_repository.UpdatePatch(id, patchDoc));
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
            return new SuccessResponse<TaskUser>(entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpDelete]
    public ActionResult Delete(TaskUser body)
    {
        try
        {
            _repository.Remove(body);
            _repository.Save();
            return new SuccessResponse<TaskUser>(body);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }


    [HttpGet]
    public ActionResult<IEnumerable<TaskEntity>> Get([FromQuery(Name = "filter")] string? filterString, int? page,
        int? pageSize, string? includes = "", string? orderBy = null)
    {
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<TaskUser>>(
            _repository.Get(CompositeFilter<TaskUser>.ApplyFilter(filter), includes, orderBy, page, pageSize));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<IEnumerable<TaskUser>> GetId(long id, string? includes = "")
    {
        try
        {
            return new SuccessResponse<TaskUser>(_repository.GetById(id.ToString(), includes));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }
}