using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Helpers;
using System.Linq.Expressions;

namespace server.Interfaces;

public interface IResourceService
{
    IEnumerable<Resource> GetAllResource();
    Resource CreateResource(Resource resource);
    public Resource GetResource(long id);
    Resource DeleteResource(long idResource);
    Resource UpdateResource(long id, [FromBody] JsonPatchDocument<Resource> patchDoc);
    Resource UpdateResource(Resource resource);
}