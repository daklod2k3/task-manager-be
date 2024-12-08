using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class ResourceController : Controller
{
    private readonly IRepository<Resource> _repository;

    public ResourceController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.Resources;
    }

    [HttpPost]
    public ActionResult Create(Resource resource)
    {
        try
        {
            var entity = _repository.Add(resource);
            _repository.Save();
            return new SuccessResponse<Resource>(entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpPut]
    public ActionResult Update(Resource body)
    {
        try
        {
            var Resource = _repository.Update(body);
            _repository.Save();
            return new SuccessResponse<Resource>(Resource);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePatch(long id, [FromBody] JsonPatchDocument<Resource> patchDoc)
    {
        return new SuccessResponse<Resource>(_repository.UpdatePatch(id, patchDoc));
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteId(long id)
    {
        try
        {
            var entity = _repository.GetById(id);
            _repository.Remove(entity);
            _repository.Save();
            return new SuccessResponse<Resource>(entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpDelete]
    public ActionResult Delete(Resource body)
    {
        try
        {
            _repository.Remove(body);
            _repository.Save();
            return new SuccessResponse<Resource>(body);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }


    [HttpGet]
    public ActionResult<IEnumerable<TaskEntity>> Get([FromQuery(Name = "filter")] string? filterString,
        string? includes, int? page, int? pageItem)
    {
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        if(includes != "")
        {
            {
                var query = _repository.GetQuery(
                    CompositeFilter<Resource>.ApplyFilter(filter)
                )
                .Include(d => d.Permissions);

                return new SuccessResponse<IEnumerable<Resource>>(query.ToList());
            }
        }

        return new SuccessResponse<IEnumerable<Resource>>(
            _repository.Get(CompositeFilter<Resource>.ApplyFilter(filter), includeProperties: includes));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<IEnumerable<Resource>> GetId(long id, string? includes)
    {
        try
        {
            return new SuccessResponse<Resource>(_repository.GetById(id.ToString(), includes));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }
}