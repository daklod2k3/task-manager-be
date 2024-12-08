using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentUserController : Controller
{
    private readonly IRepository<DepartmentUser> _repository;

    public DepartmentUserController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.DepartmentUsers;
    }

    [HttpPost]
    public ActionResult Create(DepartmentUser body)
    {
        var entity = _repository.Add(body);
        _repository.Save();
        return new SuccessResponse<DepartmentUser>(entity);
    }

    [HttpPut]
    public ActionResult Update(DepartmentUser body)
    {
        var entity = _repository.Update(body);
        _repository.Save();
        return new SuccessResponse<DepartmentUser>(entity);
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePatch(long id, [FromBody] JsonPatchDocument<DepartmentUser> patchDoc)
    {
        return new SuccessResponse<DepartmentUser>(_repository.UpdatePatch(id, patchDoc));
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteId(long id)
    {
        var entity = _repository.GetById(id.ToString());
        _repository.Remove(entity);
        _repository.Save();
        return new SuccessResponse<DepartmentUser>(entity);
    }

    [HttpDelete]
    public ActionResult Delete(DepartmentUser body)
    {
        _repository.Remove(body);
        _repository.Save();
        return new SuccessResponse<DepartmentUser>(body);
    }


    [HttpGet]
    public ActionResult Get([FromQuery(Name = "filter")] string? filterString, int? page,
        int? pageItem, string? includes = "")
    {
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<DepartmentUser>>(
            _repository.Get(CompositeFilter<DepartmentUser>.ApplyFilter(filter), includeProperties: includes));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult GetId(long id, string? includes = "")
    {
        return new SuccessResponse<DepartmentUser>(_repository.GetById(id.ToString(), includes));
    }
}