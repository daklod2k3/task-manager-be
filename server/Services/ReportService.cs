using server.Entities;
using server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Số lượng task đã hoàn thành
        public async Task<int> GetCompletedTasksCountAsync()
        {
            var completedTasks = await _unitOfWork.Task.GetAsync(task => task.Status == ETaskStatus.Done);
            return completedTasks.Count();
        }

        // Số lượng task theo trạng thái
        public async Task<Dictionary<string, int>> GetTasksCountByStatusAsync()
        {
            var tasks = await _unitOfWork.Task.GetAllAsync();
            var taskCountByStatus = tasks
                .GroupBy(task => task.Status.ToString())  // Lấy trạng thái dưới dạng chuỗi
                .ToDictionary(group => group.Key, group => group.Count());
            return taskCountByStatus;
        }

        // Số lượng task theo mức độ ưu tiên
        public async Task<Dictionary<string, int>> GetTasksCountByPriorityAsync()
        {
            var tasks = await _unitOfWork.Task.GetAllAsync();
            var taskCountByPriority = tasks
                .GroupBy(task => task.Priority.ToString())  // Lấy mức độ ưu tiên dưới dạng chuỗi
                .ToDictionary(group => group.Key, group => group.Count());
            return taskCountByPriority;
        }

        // Số lượng task theo người dùng
        public async Task<Dictionary<Guid, int>> GetTasksCountByUserAsync()
        {
            var tasks = await _unitOfWork.Task.GetAllAsync();
            var taskCountByUser = tasks
                .SelectMany(task => task.TaskUsers)  // Lấy danh sách người dùng từ TaskUsers
                .GroupBy(taskUser => taskUser.UserId)  // Giả sử TaskUser có thuộc tính UserId
                .ToDictionary(group => group.Key, group => group.Count());
            return taskCountByUser;
        }

        // Số lượng task theo phòng ban
        public async Task<Dictionary<long, int>> GetTasksCountByDepartmentAsync()
        {
            var tasks = await _unitOfWork.Task.GetAllAsync();
            var taskCountByDepartment = tasks
                .SelectMany(task => task.TaskDepartments)  // Lấy danh sách phòng ban từ TaskDepartments
                .GroupBy(taskDepartment => taskDepartment.DepartmentId)  // Giả sử TaskDepartment có thuộc tính DepartmentId
                .ToDictionary(group => group.Key, group => group.Count());
            return taskCountByDepartment;
        }

        // Số lượng task hoàn thành trong khoảng thời gian cụ thể
        public async Task<Dictionary<string, int>> GetCompletedTasksByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var completedTasks = await _unitOfWork.Task.GetAsync(
                task => task.Status == ETaskStatus.Done && task.CreatedAt >= startDate && task.CreatedAt <= endDate
            );

            var taskCountByDate = completedTasks
                .GroupBy(task => task.CreatedAt.Value.Date.ToString("yyyy-MM-dd"))  // Nhóm theo ngày
                .ToDictionary(group => group.Key, group => group.Count());  // Tạo từ điển với ngày là key và số lượng task là value

            return taskCountByDate;
        }

        Task<Dictionary<int, int>> IReportService.GetTasksCountByDepartmentAsync()
        {
            throw new NotImplementedException();
        }
    }
}
