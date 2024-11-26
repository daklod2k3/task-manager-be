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

    public ChannelUser CreateChannelUser(ChannelUser department)
    {
        var result = _unitOfWork.ChannelUser.Add(department);
        _unitOfWork.Save();
        return result;
    }

    public ChannelUser GetChannelUser(long id)
    {
        return _unitOfWork.ChannelUser.GetById(id);
    }

    public ChannelUser UpdateChannelUser(ChannelUser department)
    {
        var result = _unitOfWork.ChannelUser.Update(department);
        _unitOfWork.Save();
        return result;
    }

    public ChannelUser UpdateChannelUserPatch(long id, [FromBody] JsonPatchDocument<ChannelUser> patchDoc)
    {
        var department = _unitOfWork.ChannelUser.GetById(id);
        if (department == null) throw new Exception("not found department");

        patchDoc.ApplyTo(department);

        _unitOfWork.Save();

        return department;
    }

    public ChannelUser DeleteChannelUser(long id)
    {
        var channeluser = _unitOfWork.ChannelUser.GetById(id);
        var result = _unitOfWork.ChannelUser.Remove(channeluser);
        _unitOfWork.Save();
        return result;
    }

    public IEnumerable<ChannelUser> GetChannelUserByFilter(Expression<Func<ChannelUser, bool>> filter)
    {
        return _unitOfWork.ChannelUser.Get(filter, includeProperties: "ChannelUserUsers,TaskChannelUsers");
    }

    public IEnumerable<ChannelUser> GetAllChannelUser()
    {
        CreateChannelUser(new ChannelUser());
        return _unitOfWork.ChannelUser.Get();
    }
}