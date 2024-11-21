using Microsoft.AspNetCore.JsonPatch;
using server.Entities;
using server.Interfaces;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.AspNetCore.Mvc;

namespace server.Services
{
    public class TaskDepartmentService : ITaskDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor nhận vào IUnitOfWork
        public TaskDepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TaskDepartment CreateTaskDepartment(TaskDepartment taskDepartment)
        {
            // Gọi phương thức Add của _unitOfWork.TaskDepartment và truyền vào taskDepartment
            var result = _unitOfWork.TaskDepartment.Add(taskDepartment);
            // Gọi phương thức Save của _unitOfWork
            _unitOfWork.Save();
            // Trả về result
            return result;
        }

        public TaskDepartment DeleteTaskDepartmentById(long id)
        {
            // Gọi phương thức Get của _unitOfWork.TaskDepartment và truyền vào biểu thức lambda
            var taskDepartment = _unitOfWork.TaskDepartment.Get(x => x.Id == id);
            // Gọi phương thức Remove của _unitOfWork.TaskDepartment và truyền vào taskDepartment
            var result = _unitOfWork.TaskDepartment.Remove(taskDepartment);
            // Gọi phương thức Save của _unitOfWork
            _unitOfWork.Save();
            // Trả về result
            return result;
        }

        public IEnumerable<TaskDepartment> GetAllTaskDepartment()
        {
            // Gọi phương thức GetAll của _unitOfWork.TaskDepartment
            return _unitOfWork.TaskDepartment.GetAll();
        }

        public TaskDepartment GetTaskDepartmentById(long id)
        {
            // Gọi phương thức Get của _unitOfWork.TaskDepartment và truyền vào biểu thức lambda
            return _unitOfWork.TaskDepartment.Get(x => x.Id == id);
        }

        public TaskDepartment PatchTaskDepartmentById(long id, [FromBody] JsonPatchDocument<TaskDepartment> patchDoc)
        {
            // Gọi phương thức Get của _unitOfWork.TaskDepartment và truyền vào biểu thức lambda
            var taskDepartment = _unitOfWork.TaskDepartment.Get(x => x.Id == id);
            // Nếu taskDepartment là null thì ném ra ngoại lệ
            if (taskDepartment == null) throw new Exception("not found taskDepartment");
            // Áp dụng patchDoc vào taskDepartment
            patchDoc.ApplyTo(taskDepartment);
            // Gọi phương thức Save của _unitOfWork
            _unitOfWork.Save();
            // Trả về taskDepartment
            return taskDepartment;
        }

        public TaskDepartment UpdateTaskDepartmentById(long id, TaskDepartment taskDepartment)
        {
            // Gọi phương thức Get của _unitOfWork.TaskDepartment và truyền vào biểu thức lambda
            var taskDepartmentInDb = _unitOfWork.TaskDepartment.Get(x => x.Id == id);
            // Nếu taskDepartmentInDb là null thì ném ra ngoại lệ
            if (taskDepartmentInDb == null) throw new Exception("not found taskDepartment");
            // Cập nhật các trường của taskDepartmentInDb bằng các trường của taskDepartment
            taskDepartmentInDb.TaskId = taskDepartment.TaskId;
            taskDepartmentInDb.DepartmentId = taskDepartment.DepartmentId;
            taskDepartmentInDb.CreatedAt = taskDepartment.CreatedAt;
            // Gọi phương thức Save của _unitOfWork
            _unitOfWork.Save();
            // Trả về taskDepartmentInDb
            return taskDepartmentInDb;
        }
    }
}
