using System.Linq.Expressions;
using LinqKit;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Services
{
    public class TaskHistoryService : ITaskHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskHistoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TaskHistory CreateTaskHistory(TaskHistory taskHistory)
        {
            var result = _unitOfWork.TaskHistory.Add(taskHistory);
            _unitOfWork.Save();
            return result;
        }

        public TaskHistory DeleteTaskHistoryById(long id)
        {
            var taskHistory = _unitOfWork.TaskHistory.Get(x => x.Id == id);
            var result = _unitOfWork.TaskHistory.Remove(taskHistory);
            _unitOfWork.Save();
            return result;
        }

        public IEnumerable<TaskHistory> GetAllTaskHistory()
        {
            return _unitOfWork.TaskHistory.GetAll();
        }

        public TaskHistory GetTaskHistoryById(long id)
        {
            return _unitOfWork.TaskHistory.Get(x => x.Id == id);
        }

        public TaskHistory PatchTaskHistoryById(long id, [FromBody] JsonPatchDocument<TaskHistory> patchDoc)
        {
            var taskHistory = _unitOfWork.TaskHistory.Get(x => x.Id == id);
            if (taskHistory == null) throw new Exception("not found taskHistory");
            patchDoc.ApplyTo(taskHistory);
            _unitOfWork.Save();
            return taskHistory;
        }

        public TaskHistory UpdateTaskHistoryById(long id, TaskHistory taskHistory)
        {
            var taskHistoryInDb = _unitOfWork.TaskHistory.Get(x => x.Id == id);
            if (taskHistoryInDb == null) throw new Exception("not found taskHistory");
            taskHistoryInDb.TaskId = taskHistory.TaskId;
            taskHistoryInDb.CreatedBy = taskHistory.CreatedBy;
            taskHistoryInDb.CreatedAt = taskHistory.CreatedAt;
            taskHistoryInDb.Description = taskHistory.Description;
            _unitOfWork.Save();
            return taskHistoryInDb;
        }
    }

}