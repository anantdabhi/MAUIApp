using ChatAppApi.Function.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChatAppApi.Helpers;

public class AuthorizeAttribute: Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext?.Items["User"] as User;
        
        if (user == null)
        {
            context.Result = new JsonResult(new { StatusMessage = "Unauthorized" });
        }
        
        
    }
}