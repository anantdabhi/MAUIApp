using ChatAppApi.Function.User;

namespace ChatAppApi.Controllers.Message;

public class MessageInitializeResponse
{
    public User FriendInfo { get; set; } = null;

    public IEnumerable<Function.Message.Message> Messages { get; set; } = null;

}