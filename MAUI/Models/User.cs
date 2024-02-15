namespace ChatAppTutorial.Models;

public class User
{
    public int Id { get; set; }

    public string LoginId { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string AvatarFile { get; set; }

    public bool IsOnline { get; set; }

    public bool IsAway { get; set; }

    public string LastLogonDay { get; set; }

    public string LastLogonTime { get; set; }

    public string AwayDuration { get; set; }

    public string Age { get; set; }

    public string Hobbies { get; set; }

    public string Job { get; set; }
}