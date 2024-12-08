using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Services;

public class ChannelUserService : IChannelUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public ChannelUserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ChannelUser CreateChannelUser(ChannelUser channeluser)
    {
        var result = _unitOfWork.ChannelUsers.Add(channeluser);
        _unitOfWork.Save();
        return result;
    }

    public ChannelUser GetChannelUser(long id)
    {
        return _unitOfWork.ChannelUsers.GetById(id);
    }

    public ChannelUser UpdateChannelUser(ChannelUser channeluser)
    {
        var result = _unitOfWork.ChannelUsers.Update(channeluser);
        _unitOfWork.Save();
        return result;
    }

    public ChannelUser UpdateChannelUserPatch(long id, [FromBody] JsonPatchDocument<ChannelUser> patchDoc)
    {
        var channeluser = _unitOfWork.ChannelUsers.GetById(id);
        if (channeluser == null) throw new Exception("not found channeluser");

        patchDoc.ApplyTo(channeluser);

        _unitOfWork.Save();

        return channeluser;
    }

    public ChannelUser DeleteChannelUser(long id)
    {
        var channeluser = _unitOfWork.ChannelUsers.GetById(id);
        var result = _unitOfWork.ChannelUsers.Remove(channeluser);
        _unitOfWork.Save();
        return result;
    }

    public IEnumerable<ChannelUser> GetChannelUserByFilter(Expression<Func<ChannelUser, bool>> filter)
    {
        return _unitOfWork.ChannelUsers.Get(filter, includeProperties: "ChannelUserUsers,TaskChannelUsers");
    }

    public IEnumerable<ChannelUser> GetAllChannelUser()
    {
        CreateChannelUser(new ChannelUser());
        return _unitOfWork.ChannelUsers.Get();
    }
}