using System.ComponentModel.DataAnnotations.Schema;

namespace ChatAppApi.Entities;

[Table("tblusers")]
public class TblUser
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("loginid")]
    public string LoginId { get; set; }
    
    [Column("username")]
    public string UserName { get; set; }
    
    [Column("password")]
    public string Password { get; set; }
    
    [Column("avatarfile")]
    public string AvatarFile { get; set; }
    
    [Column("isonline")]
    public bool IsOnline { get; set; }
    
    [Column("lastlogonday")]
    public string LastLogonDay { get; set; }
    
    [Column("lastlogontime")]
    public string LastLogonTime { get; set; }
    
    [Column("age")]
    public string Age { get; set; }
    
    [Column("hobbies")]
    public string Hobbies { get; set; }
    
    [Column("job")]
    public string Job { get; set; }
}