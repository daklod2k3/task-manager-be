using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using System.Linq.Expressions;

namespace server.Interfaces
{
    public interface ITaskHistoryService
    {
        IEnumerable<TaskHistory> GetAllTaskHistory();
        TaskHistory CreateTaskHistory(TaskHistory taskHistory);
        TaskHistory GetTaskHistoryById(long id);
        TaskHistory UpdateTaskHistoryById(long id, TaskHistory taskHistory);
        TaskHistory PatchTaskHistoryById(long id,[FromBody] JsonPatchDocument<TaskHistory> patchDoc);
        TaskHistory DeleteTaskHistoryById(long id);

    }
}