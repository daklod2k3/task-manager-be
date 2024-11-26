using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using System.Linq.Expressions;

namespace server.Interfaces;
    public interface IUserMessageService
    {
        public UserMessage CreatUserMessage(UserMessage usermessage);
        public UserMessage UpdateUserMessage(UserMessage usermessage);
        public UserMessage DeleteUserMessage(long idUserMessage);
        public UserMessage UpdateUserMessagePatch(long id, [FromBody] JsonPatchDocument<UserMessage> patchDoc);
        public UserMessage GetUserMessage(long id);
        public IEnumerable<UserMessage> GetUserMessageByFilter(Expression<Func<UserMessage, bool>> compositeFilterExpression);

    }
