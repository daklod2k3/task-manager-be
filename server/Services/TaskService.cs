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

        public Tasks CreatTask(Tasks task)
        {

            var result = _unitOfWork.Task.Add(task);
            _unitOfWork.Save();
            return result;
        }
        
        public IEnumerable<Tasks> GetAllTask()
        {
            return _unitOfWork.Task.GetAll();
        }
        public int AssignTaskToUser(TaskUser[] taskUsers)
        {
            foreach (var taskUser in taskUsers) {
                _unitOfWork.TaskUser.Add(taskUser);
            }
            return _unitOfWork.Save();
        }
        public int AssignTaskToDepartment(TaskDepartment[] taskDepartments)
        {
            foreach (var taskDepartment in taskDepartments)
            {
                _unitOfWork.TaskDepartment.Add(taskDepartment);
            }
            return _unitOfWork.Save();
        }


    }
}
