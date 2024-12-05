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

    public TaskHistoryController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.TaskHistories;
    }

    [HttpPost]
    public ActionResult Create(TaskHistory body)
    {
        body.CreatedBy = new Guid(AuthController.GetUserId(HttpContext));
        var entity = _repository.Add(body);
        _repository.Save();
        return new SuccessResponse<TaskHistory>(entity);
    }

    [HttpPut]
    public ActionResult Update(TaskHistory body)
    {
        var entity = _repository.Update(body);
        _repository.Save();
        return new SuccessResponse<TaskHistory>(entity);
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
        return new SuccessResponse<TaskHistory>(entity);
    }

    [HttpDelete]
    public ActionResult Delete(TaskHistory body)
    {
        _repository.Remove(body);
        _repository.Save();
        return new SuccessResponse<TaskHistory>(body);
    }


    [HttpGet]
    public ActionResult Get([FromQuery(Name = "filter")] string? filterString, int? page,
        int? pageItem, string? includes = "")
    {
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<TaskHistory>>(
            _repository.Get(CompositeFilter<TaskHistory>.ApplyFilter(filter), includeProperties: includes));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult GetId(long id, string? includes = "")
    {
        return new SuccessResponse<TaskHistory>(_repository.GetById(id.ToString(), includes));
    }
}