using ChatAppApi.Entities;
using ChatAppApi.Function.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatAppApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ServiceController : ControllerBase
{
    private readonly ChatAppContext _db;
    private readonly IUserFunction _userFunction;

    public ServiceController(ChatAppContext chatAppContext, IUserFunction userFunction)
    {
        _db = chatAppContext;
        _userFunction = userFunction;
    }

    [HttpPost("Authenticate")]
    public IActionResult Authenticate(AuthenticateRequest request)
    {
        var response = _userFunction.Authenticate(request.LoginId, request.Password);
        if (response == null) return BadRequest(new {StatusMessage = "Invalid username or password"});
        return Ok(response);
    }

    [HttpGet]
    public List<User> GetTblUsers()
    {
        List<User> response = new List<User>();
        _db.Database.Migrate();
        var dataList = _db.TblUsers.ToList();
        
        dataList.ForEach(row => response.Add(new User()
        {
            Id = row.Id,
            LoginId = row.LoginId,
            UserName = row.UserName,
            Password = row.Password,
            AvatarFile = row.AvatarFile,
            IsOnline = row.IsOnline,
            LastLogonDay = row.LastLogonDay,
            LastLogonTime = row.LastLogonTime,
            Age = row.Age,
            Hobbies = row.Hobbies,
            Job = row.Job
        }));
        return response.OrderBy(x=> x.UserName).ToList();
    }
}