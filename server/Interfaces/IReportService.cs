using server.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.Interfaces;

    public interface IReportService
    {
        // Số lượng task đã hoàn thành
        Task<int> GetCompletedTasksCountAsync();

        // Số lượng task theo trạng thái
        Task<Dictionary<string, int>> GetTasksCountByStatusAsync();

        // Số lượng task theo mức độ ưu tiên
        Task<Dictionary<string, int>> GetTasksCountByPriorityAsync();

        // Số lượng task theo người dùng
        Task<Dictionary<Guid, int>> GetTasksCountByUserAsync();

        // Số lượng task theo phòng ban
        Task<Dictionary<int, int>> GetTasksCountByDepartmentAsync();

        // Số lượng task hoàn thành trong khoảng thời gian cụ thể
        Task<Dictionary<string, int>> GetCompletedTasksByDateRangeAsync(DateTime startDate, DateTime endDate);
    }

