using System.ComponentModel.DataAnnotations.Schema;

namespace ChatAppApi.Entities;

[Table("tbluserfriends")]
public class TblUserFriend
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("userid")]
    public int UserId { get; set; }
    
    [Column("friendid")]
    public int FriendId { get; set; }
}