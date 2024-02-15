using ChatAppTutorial.Models;

namespace ChatAppTutorial.Services.ListChat;

public class NonFriendInitializeResponse: BaseResponse
{
    public User User { get; set; }

    public IEnumerable<User> UserNonFriends { get; set; } = null;
}