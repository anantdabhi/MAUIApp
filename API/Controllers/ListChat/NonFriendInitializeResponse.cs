using ChatAppApi.Function.User;

namespace ChatAppApi.Controllers.ListChat;

public class NonFriendInitializeResponse
{
    public User User { get; set; }

    public IEnumerable<User> UserNonFriends { get; set; } = null;
}