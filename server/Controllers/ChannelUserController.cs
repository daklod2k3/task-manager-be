using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;
using server.Services;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class ChannelUserController : Controller
{
    private readonly IChannelUserService _channeluserService;

    public ChannelUserController(IChannelUserService channeluserService)
    {
        _channeluserService = channeluserService;
    }

    [HttpPost]
    public ActionResult CreateChannelUser(ChannelUser channeluser)
    {
        try
        {
            return new SuccessResponse<ChannelUser>(_channeluserService.CreateChannelUser(channeluser));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("ChannelUser is not create");
        }
    }

    [HttpPut]
    public ActionResult UpdateChannelUser(ChannelUser channeluser)
    {
        try
        {
            return new SuccessResponse<ChannelUser>(_channeluserService.UpdateChannelUser(channeluser));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("ChannelUser is not update");
        }
    }

    [HttpPatch("{id}")]
    public ActionResult UpdateChannelUserPatch(long id, [FromBody] JsonPatchDocument<ChannelUser> patchDoc)
    {
        try
        {
            return new SuccessResponse<ChannelUser>(_channeluserService.UpdateChannelUserPatch(id, patchDoc));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("channeluser is not update");
        }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteChannelUser(long id)
    {
        try
        {
            return new SuccessResponse<ChannelUser>(_channeluserService.DeleteChannelUser(id));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("channeluser is not delete");
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<IEnumerable<ChannelUser>> GetChannelUserByFilter(string filterString)
    {
        var filterResult = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString))
            filterResult = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        var compositeFilterExpression = CompositeFilter<ChannelUser>.ApplyFilter(filterResult);

        var DepartmetnList = _channeluserService.GetChannelUserByFilter(compositeFilterExpression);
        return new SuccessResponse<IEnumerable<ChannelUser>>(DepartmetnList);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ChannelUser>> Get(string? filter)
    {
        return GetChannelUserByFilter(filter);
    }

    [HttpGet]
    [Route("{channeluserId}")]
    public ActionResult<IEnumerable<ChannelUser>> GetChannelUserById(long channeluserId)
    {
        try
        {
            return new SuccessResponse<ChannelUser>(_channeluserService.GetChannelUser(channeluserId));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("channeluser is not get");
        }
    }
}