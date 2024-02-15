using ChatAppApi.Entities;
using ChatAppApi.Function.Message;
using ChatAppApi.Function.User;
using ChatAppApi.Function.UserFriend;
using ChatAppApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatAppApi.Controllers.ListChat;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ListChatController : ControllerBase
{
    private IUserFunction _userFunction;
    private IUserFriendFunction _userFriendFunction;
    private IMessageFunction _messageFunction;
    private ChatAppContext _chatAppContext;

    public ListChatController(IUserFunction userFunction, IUserFriendFunction userFriendFunction,
        IMessageFunction messageFunction, ChatAppContext chatAppContext)
    {
        _userFunction = userFunction;
        _userFriendFunction = userFriendFunction;
        _messageFunction = messageFunction;
        _chatAppContext = chatAppContext;
    }

    [HttpPost("Initialize")]
    public async Task<ActionResult> Initialize([FromBody] int userId)
    {
        var list = await _userFriendFunction.GetPotentialUserFriend(userId);
        var response = new ListChatInitializeResponse
        {
            User = _userFunction.GetUserById(userId),
            UserFriends = await _userFriendFunction.GetListUserFriend(userId),
            LatestMessages = await _messageFunction.GetLatestMessage(userId),
            HasPossibleFriend = (await _userFriendFunction.GetPotentialUserFriend(userId)).ToList().Count != 0
        };

        return Ok(response);
    }

    [HttpPost("GetNotFriends")]
    public async Task<ActionResult> GetNotFriends([FromBody] int userId)
    {
        var response = new NonFriendInitializeResponse
        {
            User = _userFunction.GetUserById(userId),
            UserNonFriends = await _userFriendFunction.GetListNonUserFriend(userId)
        };

        return Ok(response);
    }

    [HttpPost("AddFriend")]
    public async Task<int> AddFriend(AddFriendRequest request)
    {
        var list = await _chatAppContext.TblUserFriends
            .OrderBy(x => x.Id).ToListAsync();
        _chatAppContext.TblUserFriends.Add(new TblUserFriend
        {
            Id = list[list.Count-1].Id + 1,
            UserId = int.Parse(request.UserId),
            FriendId = int.Parse(request.FriendId)
        });
        var result = await _chatAppContext.SaveChangesAsync();
        return result;
    }

    [HttpPost("GetPotentialFriends")]
    public async Task<ActionResult> GetPotentialFriends([FromBody] int userId)
    {
        var response = new NonFriendInitializeResponse
        {
            User = _userFunction.GetUserById(userId),
            UserNonFriends = await _userFriendFunction.GetPotentialUserFriend(userId)
        };

        return Ok(response);
    }
}