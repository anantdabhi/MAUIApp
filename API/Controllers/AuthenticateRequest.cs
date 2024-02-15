namespace ChatAppApi.Controllers;

public class AuthenticateRequest
{
    public string LoginId { get; set; }
    
    public string Password { get; set; }
}