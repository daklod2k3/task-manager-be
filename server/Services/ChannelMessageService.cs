using System.Linq.Expressions;
using LinqKit;
using server.Entities;
using server.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace server.Services;

public class ChannelMessageService : IChannelMessageService
{
    private readonly IUnitOfWork _unitOfWork;

    public ChannelMessageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ChannelMessage CreateChannelMessage(ChannelMessage ChannelMessage)
    {
        var result = _unitOfWork.ChannelMessages.Add(ChannelMessage);
        _unitOfWork.Save();
        return result;
    }

    public IEnumerable<ChannelMessage> GetAllChannelMessage()
    {
        CreateChannelMessage(new ChannelMessage());
        return _unitOfWork.ChannelMessages.Get();
    }

    public ChannelMessage DeleteChannelMessage(long id)
    {
        var ChannelMessage = _unitOfWork.ChannelMessages.GetById(id);
        var result = _unitOfWork.ChannelMessages.Remove(ChannelMessage);
        _unitOfWork.Save();
        return result;
    }

    public ChannelMessage UpdateChannelMessage(ChannelMessage ChannelMessage)
    {
        var result = _unitOfWork.ChannelMessages.Update(ChannelMessage);
        return result;
    }

    public ChannelMessage PatchChannelMessage(long id,[FromBody] JsonPatchDocument<ChannelMessage> ChannelMessage)
    {
        var channelmessage = _unitOfWork.ChannelMessages.GetById(id);
        if (channelmessage == null) throw new Exception("ChannelMessage not found");

        ChannelMessage.ApplyTo(channelmessage);

        _unitOfWork.Save();

        return channelmessage;
    }


    public ChannelMessage GetChannelMessage(long id)
    {
        return _unitOfWork.ChannelMessages.GetById(id);
    }

    public IEnumerable<ChannelMessage> GetChannelMessageByFilter(Expression<Func<ChannelMessage, bool>> filter)
    {
        return _unitOfWork.ChannelMessages.Get(filter, includeProperties: "Channel");
    }

    
}