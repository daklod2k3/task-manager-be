using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskDepartmentController : Controller
{
    private readonly IRepository<TaskDepartment> _repository;

    public TaskDepartmentController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.TaskDepartments;
    }

    [HttpPost]
    public ActionResult Create(TaskDepartment body)
    {
        var entity = _repository.Add(body);
        _repository.Save();
        return new SuccessResponse<TaskDepartment>(entity);
    }

    [HttpPut]
    public ActionResult Update(TaskDepartment body)
    {
        var entity = _repository.Update(body);
        _repository.Save();
        return new SuccessResponse<TaskDepartment>(entity);
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<TaskDepartment> patchDoc)
    {
        return new SuccessResponse<TaskDepartment>(_repository.UpdatePatch(id, patchDoc));
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteId(long id)
    {
        var entity = _repository.GetById(id.ToString());
        _repository.Remove(entity);
        _repository.Save();
        return new SuccessResponse<TaskDepartment>(entity);
    }

    [HttpDelete]
    public ActionResult Delete(TaskDepartment body)
    {
        _repository.Remove(body);
        _repository.Save();
        return new SuccessResponse<TaskDepartment>(body);
    }


    [HttpGet]
    public ActionResult Get([FromQuery(Name = "filter")] string? filterString, int? page,
        int? pageItem, string? includes = "")
    {
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<TaskDepartment>>(
            _repository.Get(CompositeFilter<TaskDepartment>.ApplyFilter(filter), includeProperties: includes));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult GetId(long id, string? includes = "")
    {
        return new SuccessResponse<TaskDepartment>(_repository.GetById(id.ToString(), includes));
    }
}