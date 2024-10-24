﻿using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;
using System.Threading.Tasks;

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
        public Tasks DeleteTask(long id)
        {
            var task = _unitOfWork.Task.Get(x => x.Id == id);
            var result = _unitOfWork.Task.Remove(task);
            _unitOfWork.Save();
            return result;
        }

        public Tasks UpdateTask(Tasks task)
        {
            var result = _unitOfWork.Task.Update(task);
            _unitOfWork.Save();
            return result;
        }

        public TaskDepartment UpdateAssignTaskToDepartment(TaskDepartment taskDepartment)
        {
            var result = _unitOfWork.TaskDepartment.Update(taskDepartment);
            _unitOfWork.Save();
            return result;
        }

        public TaskDepartment DeleteAssignTaskToDepartment(long id)
        {
            var taskDepartment = _unitOfWork.TaskDepartment.Get(x => x.Id == id);
            var result = _unitOfWork.TaskDepartment.Remove(taskDepartment);
            _unitOfWork.Save();
            return result;
        }

        public TaskUser UpdateAssignTaskToUser(TaskUser taskUser)
        {
            var result = _unitOfWork.TaskUser.Update(taskUser);
            _unitOfWork.Save();
            return result;
        }

        public TaskUser DeleteAssignTaskToUser(long id)
        {
            var taskUser = _unitOfWork.TaskUser.Get(x => x.Id == id);
            var result = _unitOfWork.TaskUser.Remove(taskUser);
            _unitOfWork.Save();
            return result;
        }
    }
}
