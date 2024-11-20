using System.Linq.Expressions;
using LinqKit;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Services
{
    public class TaskUserService : ITaskUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<TaskUser> GetAllTaskUsers()
        {
            return _unitOfWork.TaskUser.GetAll();
        }

        public TaskUser? GetTaskUserById(int id)
        {
            return _unitOfWork.TaskUser.GetById(id);
        }

        public void CreateTaskUser(TaskUser taskUser)
        {
            _unitOfWork.TaskUser.Add(taskUser);
            _unitOfWork.Save();
        }

        public bool UpdateTaskUser(int id, TaskUser updatedTaskUser)
        {
            var taskUser = _unitOfWork.TaskUser.GetById(id);
            if (taskUser == null) return false;

            taskUser.TaskId = updatedTaskUser.TaskId;
            taskUser.UserId = updatedTaskUser.UserId;
            taskUser.CreatedAt = updatedTaskUser.CreatedAt;

            _unitOfWork.TaskUser.Update(taskUser);
            _unitOfWork.Save();
            return true;
        }

        public bool DeleteTaskUser(int id)
        {
            var taskUser = _unitOfWork.TaskUser.GetById(id);
            if (taskUser == null) return false;

            _unitOfWork.TaskUser.Remove(taskUser);
            _unitOfWork.Save();
            return true;
        }
    }
}
