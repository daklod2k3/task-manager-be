using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreatTask(Tasks task)
        {
            _unitOfWork.Task.Add(task);
            _unitOfWork.Save();
        }

        public IEnumerable<Tasks> GetAllTask()
        {
            return _unitOfWork.Task.GetAll();
        }
    }
}
