namespace ChatAppApi.Function.UserFriend;

public interface IUserFriendFunction
{
    Task<IEnumerable<User.User>> GetListUserFriend(int userId);
    Task<IEnumerable<User.User>> GetListNonUserFriend(int userId);

    Task<IEnumerable<User.User>> GetPotentialUserFriend(int userId);
}