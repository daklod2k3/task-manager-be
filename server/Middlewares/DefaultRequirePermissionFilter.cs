using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Graph.Communications.Common;
using server.Context;
using server.Controllers;

namespace server.Middlewares;

public class DefaultRequirePermissionFilter : IAuthorizationFilter
{
    private readonly SupabaseContext _dbContext;

    public DefaultRequirePermissionFilter(SupabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Check if the action or controller is excluded
        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

        if (actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any() ||
            actionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any() ||
            actionDescriptor.MethodInfo.GetCustomAttributes(typeof(ExcludePermissionAttribute), true).Any())
            return; // Skip permission check

        var userId = new Guid(AuthController.GetUserId(context.HttpContext));
        if (userId == null)
        {
            context.Result = new ForbidResult();
            return;
        }
        
        if (_dbContext.Profiles.Any(u=>u.Id == userId && u.RoleId == 0))
            return;

        try
        {
            var pathname = context.HttpContext.Request.Path.Value.Split("/").ToList();
            // pathname.RemoveAt(pathname.Count - 1);

            var actionMethod = context.HttpContext.Request.Method;
            var requiredResource = pathname.Join("/");
            var resources = _dbContext.Resources.ToList()
                .Where(r =>
                    r.Path.ToLower().StartsWith(requiredResource.ToLower()) ||
                    (requiredResource.ToLower().StartsWith(r.Path.ToLower()) && requiredResource[r.Path.Length] == '/'))
                .Select(r => r.Id);
            if (resources != null)
            {
                var userPermissions = _dbContext.Roles
                    .Where(r => r.Profiles.Any(u => u.Id == userId))
                    .SelectMany(r => r.Permissions)
                    .Where(p => resources.Contains(p.ResourceId)).ToList();
                foreach (var permission in userPermissions)
                    if (permission.GetPropertyUsingReflection(PermissionMethodMap(actionMethod)).Equals(true))
                        return;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
       

        context.Result = new ForbidResult();
    }

    public string PermissionMethodMap(string method)
    {
        switch (method)
        {
            case "GET":
                return "View";
            case "POST":
                return "Create";
            case "PUT":
            case "PATCH":
                return "Update";
            case "DELETE":
                return "Delete";
        }

        return "View";
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExcludePermissionAttribute : Attribute
    {
    }
}