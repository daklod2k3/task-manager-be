using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Services;

public class UserMessageService : IUserMessageService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserMessageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public UserMessage CreatUserMessage(UserMessage usermessage)
    {
        var result = _unitOfWork.UserMessages.Add(usermessage);
        _unitOfWork.Save();
        return result;
    }

    public UserMessage GetUserMessage(long id)
    {
        return _unitOfWork.UserMessages.GetById(id);
    }

    public UserMessage UpdateUserMessage(UserMessage usermessage)
    {
        var result = _unitOfWork.UserMessages.Update(usermessage);
        _unitOfWork.Save();
        return result;
    }

    public UserMessage UpdateUserMessagePatch(long id, [FromBody] JsonPatchDocument<UserMessage> patchDoc)
    {
        var usermessage = _unitOfWork.UserMessages.GetById(id);
        if (usermessage == null) throw new Exception("not found usermessage");

        patchDoc.ApplyTo(usermessage);

        _unitOfWork.Save();

        return usermessage;
    }

    public UserMessage DeleteUserMessage(long id)
    {
        var usermessage = _unitOfWork.UserMessages.GetById(id);
        var result = _unitOfWork.UserMessages.Remove(usermessage);
        _unitOfWork.Save();
        return result;
    }

    public IEnumerable<UserMessage> GetUserMessageByFilter(Expression<Func<UserMessage, bool>> filter)
    {
        return _unitOfWork.UserMessages.Get(filter, includeProperties: "From,To");
    }

    public IEnumerable<UserMessage> GetAllUserMessage()
    {
        CreatUserMessage(new UserMessage());
        return _unitOfWork.UserMessages.Get();
    }
}