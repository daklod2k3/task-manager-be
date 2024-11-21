using System.Linq.Expressions;
using LinqKit;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace server.Services
{
    public class TaskUserService : ITaskUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TaskUser CreateTaskUser(TaskUser taskUser)
        {
            var result = _unitOfWork.TaskUser.Add(taskUser);
            _unitOfWork.Save();
            return result;
        }

        public TaskUser DeleteTaskUserById(long id)
        {
            var taskUser = _unitOfWork.TaskUser.Get(x => x.Id == id);
            var result = _unitOfWork.TaskUser.Remove(taskUser);
            _unitOfWork.Save();
            return result;
        }

        public IEnumerable<TaskUser> GetAllTaskUsers()
        {
            return _unitOfWork.TaskUser.GetAll();
        }

        public TaskUser GetTaskUserById(long id)
        {
            return _unitOfWork.TaskUser.Get(x => x.Id == id);
        }

        public TaskUser PatchTaskUserById(long id, [FromBody] JsonPatchDocument<TaskUser> patchDoc)
        {
            var taskUser = _unitOfWork.TaskUser.Get(x => x.Id == id);
            if (taskUser == null) throw new Exception("not found taskUser");
            patchDoc.ApplyTo(taskUser);
            _unitOfWork.Save();
            return taskUser;
        }

        public TaskUser UpdateTaskUserById(long id, TaskUser taskUser)
        {
            var taskUserInDb = _unitOfWork.TaskUser.Get(x => x.Id == id);
            if (taskUserInDb == null) throw new Exception("not found taskUser");
            taskUserInDb.UserId = taskUser.UserId;
            taskUserInDb.TaskId = taskUser.TaskId;
            taskUserInDb.CreatedAt = taskUser.CreatedAt;
            _unitOfWork.Save();
            return taskUserInDb;
        }
    }
}
