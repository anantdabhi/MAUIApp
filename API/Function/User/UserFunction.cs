using ChatAppApi.Entities;

namespace ChatAppApi.Function.User;

public class UserFunction : IUserFunction
{
    private readonly ChatAppContext _chatAppContext;

    public UserFunction(ChatAppContext chatAppContext)
    {
        _chatAppContext = chatAppContext;
    }

    public User? Authenticate(string loginId, string password)
    {
        try
        {
            var entity = _chatAppContext.TblUsers.SingleOrDefault(
                x => x.LoginId == loginId
            );
            if (entity == null) return null;

            var isPasswordMatched = password == entity.Password;

            if (!isPasswordMatched) return null;

            return new User()
            {
                Id = entity.Id,
                LoginId = entity.LoginId,
                UserName = entity.UserName,
                Password = entity.Password,
                AvatarFile = entity.AvatarFile,
                IsOnline = entity.IsOnline,
                LastLogonDay = entity.LastLogonDay,
                LastLogonTime = entity.LastLogonTime,
                Age = entity.Age,
                Hobbies = entity.Hobbies,
                Job = entity.Job
            };
        }
        catch (Exception)
        {
            return null;
        }
    }

    public User GetUserById(int id)
    {
        var entity = _chatAppContext.TblUsers
            .FirstOrDefault(x => x.Id == id);

        string awayDuration;
        double result = 0;

        foreach (char c in entity.UserName)
        {
            result = Math.Pow(1.25, c - 'a');
        }

        if (result < 60)
        {
            awayDuration = ((long)result) + "m";
        }
        else
        {
            awayDuration = ((long)result / 60) + "h";
        }

        return new User
        {
            UserName = entity.UserName,
            Id = entity.Id,
            AvatarFile = entity.AvatarFile,
            AwayDuration = awayDuration,
            IsOnline = entity.IsOnline,
            IsAway = !entity.IsOnline,
            LastLogonDay = entity.LastLogonDay,
            LastLogonTime = entity.LastLogonTime,
            Password = entity.Password,
            LoginId = entity.LoginId,
            Age = entity.Age,
            Hobbies = entity.Hobbies,
            Job = entity.Job
        };
    }
}