using ChatAppApi.Function.Message;
using ChatAppApi.Function.User;

namespace ChatAppApi.Controllers.ListChat;

public class ListChatInitializeResponse
{
    public User User { get; set; }

    public IEnumerable<User> UserFriends { get; set; } = null;

    public IEnumerable<LatestMessage> LatestMessages { get; set; } = null;
    
    public bool HasPossibleFriend { get; set; } 
}