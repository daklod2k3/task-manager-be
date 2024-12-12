using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentController : Controller
{
    private readonly IRepository<Department> _repository;

    public DepartmentController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.Departments;
    }

    [HttpPost]
    public ActionResult Create(Department body)
    {
        var entity = _repository.Add(body);
        _repository.Save();
        return new SuccessResponse<Department>(entity);
    }

    [HttpPut]
    public ActionResult Update(Department body)
    {
        var entity = _repository.Update(body);
        _repository.Save();
        return new SuccessResponse<Department>(entity);
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePatch(long id, [FromBody] JsonPatchDocument<Department> patchDoc)
    {
        return new SuccessResponse<Department>(_repository.UpdatePatch(id, patchDoc));
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteId(long id)
    {
        var entity = _repository.GetById(id.ToString());
        _repository.Remove(entity);
        _repository.Save();
        return new SuccessResponse<Department>(entity);
    }

    [HttpDelete]
    public ActionResult Delete(Department body)
    {
        _repository.Remove(body);
        _repository.Save();
        return new SuccessResponse<Department>(body);
    }


    [HttpGet]
    public ActionResult Get([FromQuery(Name = "filter")] string? filterString, int? page,
        int? pageItem, string? includes = "")
    {
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<Department>>(
            _repository.Get(CompositeFilter<Department>.ApplyFilter(filter), includes));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult GetId(long id, string? includes = "")
    {
        return new SuccessResponse<Department>(_repository.GetById(id.ToString(), includes));
    }
}