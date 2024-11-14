using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // Số lượng task đã hoàn thành
        [HttpGet("completedTasksCount")]
        public async Task<IActionResult> GetCompletedTasksCount()
        {
            try
            {
                var count = await _reportService.GetCompletedTasksCountAsync();
                return Ok(new { count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Số lượng task theo trạng thái
        [HttpGet("tasksCountByStatus")]
        public async Task<IActionResult> GetTasksCountByStatus()
        {
            try
            {
                var result = await _reportService.GetTasksCountByStatusAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Số lượng task theo mức độ ưu tiên
        [HttpGet("tasksCountByPriority")]
        public async Task<IActionResult> GetTasksCountByPriority()
        {
            try
            {
                var result = await _reportService.GetTasksCountByPriorityAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Số lượng task theo người dùng
        [HttpGet("tasksCountByUser")]
        public async Task<IActionResult> GetTasksCountByUser()
        {
            try
            {
                var result = await _reportService.GetTasksCountByUserAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Số lượng task theo phòng ban
        [HttpGet("tasksCountByDepartment")]
        public async Task<IActionResult> GetTasksCountByDepartment()
        {
            try
            {
                var result = await _reportService.GetTasksCountByDepartmentAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Số lượng task hoàn thành trong khoảng thời gian cụ thể
        [HttpGet("completedTasksByDateRange")]
        public async Task<IActionResult> GetCompletedTasksByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var result = await _reportService.GetCompletedTasksByDateRangeAsync(startDate, endDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
