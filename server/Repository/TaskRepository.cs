using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;  // Ensure this is included
using server.Context;
using server.Entities;
using server.Interfaces;
public class TaskRepository : ITaskRepository
{
    private readonly SupabaseContext _context;

    // Constructor nhận vào SupabaseContext
    public TaskRepository(SupabaseContext context)
    {
        _context = context;
    }


    // Phương thức lấy tất cả task
    // Phương thức lấy tất cả task
    public async Task<List<ETask>> GetAllAsync()
    {
        try
        {
            // Trả về tất cả task từ cơ sở dữ liệu
            return await _context.Tasks.ToListAsync();
        }
        catch (Exception ex)
        {
            // Xử lý lỗi (nếu có)
            throw new Exception("Error retrieving tasks", ex);
        }
    }



    // Phương thức tìm task theo điều kiện
    public async Task<List<ETask>> GetAsync(Expression<Func<ETask, bool>> predicate)
    {
        try
        {
            // Trả về các task thỏa mãn điều kiện
            return await _context.Tasks.Where(predicate).ToListAsync();
        }
        catch (Exception ex)
        {
            // Xử lý lỗi (nếu có)
            throw new Exception("Error retrieving tasks with condition", ex);
        }
    }
}
