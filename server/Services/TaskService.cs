﻿// using System.Linq.Expressions;
// using Microsoft.AspNetCore.JsonPatch;
// using Microsoft.AspNetCore.Mvc;
// using server.Entities;
// using server.Helpers;
// using server.Interfaces;
//
// namespace server.Services;
//
// public class TaskService : ITaskService
// {
//     private readonly IUnitOfWork _unitOfWork;
//
//     public TaskService(IUnitOfWork unitOfWork)
//     {
//         _unitOfWork = unitOfWork;
//     }
//
//     public TaskEntity CreatTask(TaskEntity taskEntity)
//     {
//         var result = _unitOfWork.Tasks.Add(taskEntity);
//         _unitOfWork.Save();
//         return result;
//     }
//
//     public TaskEntity GetTask(long idTask, string? includes)
//     {
//         return _unitOfWork.Tasks.Get(x => x.Id == idTask, includes);
//     }
//
//     public IEnumerable<TaskEntity> GetAllTask()
//     {
//         return _unitOfWork.Tasks.Get();
//     }
//
//     public TaskUser AssignTaskToUser(TaskUser taskUser)
//     {
//         var result = _unitOfWork.TaskUsers.Add(taskUser);
//         _unitOfWork.Save();
//         return result;
//     }
//
//     public TaskDepartment AssignTaskToDepartment(TaskDepartment taskDepartment)
//     {
//         var result = _unitOfWork.TaskDepartments.Add(taskDepartment);
//         _unitOfWork.Save();
//         return result;
//     }
//
//     public TaskEntity DeleteTask(long id)
//     {
//         var task = _unitOfWork.Task.Get(x => x.Id == id);
//         var result = _unitOfWork.Task.Remove(task);
//         _unitOfWork.Save();
//         return result;
//     }
//
//     public TaskEntity UpdateTask(long id, [FromBody] JsonPatchDocument<TaskEntity> patchDoc)
//     {
//         var task = _unitOfWork.Task.Get(x => x.Id == id);
//         if (task == null) throw new Exception("not found task");
//
//         patchDoc.ApplyTo(task);
//
//         _unitOfWork.Save();
//
//         return task;
//     }
//
//     public TaskDepartment UpdateAssignTaskToDepartment(TaskDepartment taskDepartment)
//     {
//         var result = _unitOfWork.TaskDepartment.Update(taskDepartment);
//         _unitOfWork.Save();
//         return result;
//     }
//
//     public TaskDepartment DeleteAssignTaskToDepartment(long id)
//     {
//         var taskDepartment = _unitOfWork.TaskDepartment.Get(x => x.Id == id);
//         var result = _unitOfWork.TaskDepartment.Remove(taskDepartment);
//         _unitOfWork.Save();
//         return result;
//     }
//
//     public TaskUser UpdateAssignTaskToUser(TaskUser taskUser)
//     {
//         var result = _unitOfWork.TaskUser.Update(taskUser);
//         _unitOfWork.Save();
//         return result;
//     }
//
//     public TaskUser DeleteAssignTaskToUser(long id)
//     {
//         var taskUser = _unitOfWork.TaskUser.Get(x => x.Id == id);
//         var result = _unitOfWork.TaskUser.Remove(taskUser);
//         _unitOfWork.Save();
//         return result;
//     }
//
//     public IEnumerable<TaskEntity> GetAllTask(Expression<Func<TaskEntity, bool>>? filter, string? orderBy,
//         string? includeProperties, Pagination? pagination)
//     {
//         filter ??= t => true;
//         var query = _unitOfWork.Task.GetQuery(filter, orderBy, includeProperties);
//         return query.Paginate(pagination).ToList();
//     }
//
//     public IEnumerable<TaskEntity> GetTaskByFilter(Expression<Func<TaskEntity, bool>> filter)
//     {
//         return _unitOfWork.Task.GetAll(filter);
//     }
//
//     public TaskEntity UpdateTask(TaskEntity taskEntity)
//     {
//         var result = _unitOfWork.Task.Update(taskEntity);
//         _unitOfWork.Save();
//         return result;
//     }
// }

