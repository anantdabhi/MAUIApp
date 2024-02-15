using ChatAppApi.Function.User;

namespace ChatAppApi.Helpers;

public class UserOperator
{
    private IHttpContextAccessor _httpContext;

    public UserOperator(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    public User GetRequestUser()
    {
        if (_httpContext == null)
            return null;
        
        return _httpContext.HttpContext?.Items["User"] as User;
    }
}