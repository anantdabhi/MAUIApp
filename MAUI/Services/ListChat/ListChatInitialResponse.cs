using ChatAppTutorial.Models;

namespace ChatAppTutorial.Services.ListChat;

public class ListChatInitialResponse: BaseResponse
{
    public User User { get; set; }
    
    public IEnumerable<User> UserFriends { get; set; }
    
    public IEnumerable<LatestMessage> LatestMessages { get; set; }
    
    public bool HasPossibleFriend { get; set; }
}