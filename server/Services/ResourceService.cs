using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Services;

public class ResourceService : IResourceService
{
    private readonly IUnitOfWork _unitOfWork;

    public ResourceService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Resource CreateResource(Resource resource)
    {
        var result = _unitOfWork.Resources.Add(resource);
        _unitOfWork.Save();
        return result;
    }

    public Resource GetResource(long id)
    {
        return _unitOfWork.Resources.GetById(id);
    }

    public Resource UpdateResource(Resource resource)
    {
        var result = _unitOfWork.Resources.Update(resource);
        _unitOfWork.Save();
        return result;
    }

    public Resource UpdateResource(long id, [FromBody] JsonPatchDocument<Resource> patchDoc)
    {
        var resource = _unitOfWork.Resources.GetById(id);
        if (resource == null) throw new Exception("not found resource");

        patchDoc.ApplyTo(resource);

        _unitOfWork.Save();

        return resource;
    }

    public Resource DeleteResource(long id)
    {
        var resource = _unitOfWork.Resources.GetById(id);
        var result = _unitOfWork.Resources.Remove(resource);
        _unitOfWork.Save();
        return result;
    }

    public IEnumerable<Resource> GetAllResource()
    {
        CreateResource(new Resource());
        return _unitOfWork.Resources.Get();
    }
}