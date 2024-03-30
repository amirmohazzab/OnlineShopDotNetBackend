

using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.CustomAttribute;

public class AccessControlAttribute : ActionFilterAttribute
{
    public string Permission { get; set; }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userId = Guid.Parse(context.HttpContext.User.Claims.FirstOrDefault(q => q.Type == "userId").Value);

        var permissionService = context.HttpContext.RequestServices.GetService<IPermissionService>();

        if(!await permissionService.CheckPermission(userId, Permission))
        {
            context.Result = new BadRequestObjectResult("no access in action");
        } 
        else
        {
            base.OnActionExecutionAsync(context, next);
        }
        
    }
}

