namespace ChatAppApi.Function.Message;

public class LatestMessage
{
    public int Id { get; set; }
    
    public int UserId { get; set; }

    public User.User UserFriendInfo { get; set; } = null;

    public string Content { get; set; } = null;

    public String SendDateDay { get; set; } = null;

    public String SendDateTime { get; set; } = null;
    
    public bool IsRead { get; set; }
}