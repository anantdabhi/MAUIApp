using ChatAppApi.Function.User;

namespace ChatAppApi.Helpers;

public class JWTMiddleware
{
    private readonly RequestDelegate _next;

    public JWTMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserFunction userFunction)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token == null)
            token = context.Request.Headers["ChatHubBearer"].FirstOrDefault()?.Split(" ").Last();
        
        if (token != null)
            context.Items["User"] = userFunction.GetUserById(int.Parse(token));

        await _next(context);
    }
}