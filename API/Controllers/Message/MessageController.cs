using ChatAppApi.Function.Message;
using ChatAppApi.Function.User;
using ChatAppApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppApi.Controllers.Message;

[ApiController]
[Route("[controller]")]
[Helpers.Authorize]
public class MessageController : Controller
{
    private IMessageFunction _messageFunction;
    private IUserFunction _userFunction;

    public MessageController(IMessageFunction messageFunction, IUserFunction userFunction)
    {
        _messageFunction = messageFunction;
        _userFunction = userFunction;
    }

    [HttpPost("Initialize")]
    public async Task<ActionResult> Initialize([FromBody] MessageInitializeRequest request)
    {
        var response = new MessageInitializeResponse
        {
            FriendInfo = _userFunction.GetUserById(request.ToUserId),
            Messages = await _messageFunction.GetMessages(request.FromUserId, request.ToUserId)
        };

        return Ok(response);
    }
}