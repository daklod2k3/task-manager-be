using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class RoleController : Controller
{
    private readonly IRepository<Role> _repository;

    public RoleController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.Role;
    }

    [HttpPost]
    public ActionResult Create(Role role)
    {
        try
        {
            var entity = _repository.Add(role);
            _repository.Save();
            return new SuccessResponse<Role>(entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpPut]
    public ActionResult Update(Role body)
    {
        try
        {
            var role = _repository.Update(body);
            _repository.Save();
            return new SuccessResponse<Role>(role);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<Role> patchDoc)
    {
        try
        {
            return new SuccessResponse<Role>(_repository.UpdatePatch(id.ToString(), patchDoc));
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
    public ActionResult Delete(Role body)
    {
        try
        {
            _repository.Remove(body);
            _repository.Save();
            return new SuccessResponse<Role>(body);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }


    [HttpGet]
    public ActionResult<IEnumerable<Role>> Get([FromQuery(Name = "filter")] string? filterString,
        string? includes, int? page, int? pageItem)
    {
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<Role>>(
            _repository.Get(CompositeFilter<Role>.ApplyFilter(filter), includeProperties: includes));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<IEnumerable<Role>> GetId(long id, string? includes)
    {
        try
        {
            return new SuccessResponse<Role>(_repository.GetById(id.ToString(), includes ?? "*"));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }
}