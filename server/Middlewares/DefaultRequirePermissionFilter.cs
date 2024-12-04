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

        try
        {
            var pathname = context.HttpContext.Request.Path.Value;
            var actionMethod = context.HttpContext.Request.Method;
            var requiredResource = pathname;
            var resource = _dbContext.Resources.FirstOrDefault(r =>  requiredResource.StartsWith(r.Path));
            if (resource != null)
            {
                var userPermissions = _dbContext.Roles
                    .Where(r => r.Profiles.Any(u => u.Id == userId))
                    .SelectMany(r => r.Permissions)
                    .FirstOrDefault(p => p.ResourceId == resource.Id);
                if (userPermissions.GetPropertyUsingReflection(PermissionMethodMap(actionMethod)).Equals(true))
                    return;
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            context.Result = new ForbidResult();
            throw;
        }
        // Default permission: "ControllerName.ActionName"


        // .SelectMany(u => u.Role.Permissions)
        // .Where(p => p.Resource.Path.ToLower().Equals(requiredResource))
        // .Include(p => p.Role)


        // foreach (var per in userPermissions)
        //     if (per.GetPropertyUsingReflection(PermissionMethodMap(actionMethod)).Equals(true))

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