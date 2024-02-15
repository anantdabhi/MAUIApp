namespace ChatAppTutorial.Models;

public class LatestMessage
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public User UserFriendInfo { get; set; }
    
    public string Content { get; set; }
    
    public DateTime SendDataTime { get; set; }
    
    public bool IsRead { get; set; }   
}