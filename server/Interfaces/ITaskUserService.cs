using server.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;


namespace server.Interfaces
{
    public interface ITaskUserService
    {
        IEnumerable<TaskUser> GetAllTaskUsers();
        TaskUser CreateTaskUser(TaskUser taskUser);
        TaskUser GetTaskUserById(long id);
        TaskUser UpdateTaskUserById(long id, TaskUser taskUser);
        TaskUser PatchTaskUserById(long id, [FromBody] JsonPatchDocument<TaskUser> patchDoc);
        TaskUser DeleteTaskUserById(long id);
    }
}
