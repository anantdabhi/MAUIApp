using ChatAppApi.Entities;
using ChatAppApi.Function.User;
using Microsoft.EntityFrameworkCore;

namespace ChatAppApi.Function.UserFriend;

public class UserFriendFunction : IUserFriendFunction
{
    private ChatAppContext _chatAppContext;
    private IUserFunction _userFunction;

    public UserFriendFunction(ChatAppContext chatAppContext, IUserFunction userFunction)
    {
        _chatAppContext = chatAppContext;
        _userFunction = userFunction;
    }

    public async Task<IEnumerable<User.User>> GetListUserFriend(int userId)
    {
        var entities = await _chatAppContext.TblUserFriends
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.Id)
            .ToListAsync();

        var result = entities.Select(x => _userFunction.GetUserById(x.FriendId));

        if (result == null) result = new List<User.User>();

        return result;
    }

    public async Task<IEnumerable<User.User>> GetListNonUserFriend(int userId)
    {
        var users = await _chatAppContext.TblUsers
            .Where(x => x.Id != userId)
            .OrderBy(x => x.Id)
            .ToListAsync();
        
        
        var entities = await _chatAppContext.TblUserFriends
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.Id)
            .ToListAsync();

        for (int i = 0; i < users.Count; i++)
        {
            bool isFriend = false;
            
            for (int j = 0; j < entities.Count; j++)
            {
                if (users[i].Id == entities[j].FriendId)
                {
                    isFriend = true;
                    j = entities.Count;
                }
            }

            if (isFriend)
            {
                users.RemoveAt(i);
                i--;
            }
        }

        var result = users.Select(x => _userFunction.GetUserById(x.Id));

        if (result == null) result = new List<User.User>();

        return result;
    }

    public async Task<IEnumerable<User.User>> GetPotentialUserFriend(int userId)
    {
        var entities = await _chatAppContext.TblUserFriends
            .Where(x => x.FriendId == userId)
            .OrderBy(x => x.Id).ToListAsync();

        var friends = await _chatAppContext.TblUserFriends
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.Id).ToListAsync();

        for (int i = 0; i < entities.Count; i++)
        {
            bool flag = false;
            for (int j = 0; j < friends.Count; j++)
            {
                if (entities[i].UserId == friends[j].FriendId)
                {
                    entities.RemoveAt(i);
                    friends.RemoveAt(j);
                    j = friends.Count;
                    i--;
                }
            }
        }
        
        var result = entities.Select(x => _userFunction.GetUserById(x.UserId));

        if (result == null) result = new List<User.User>();

        return result;
    }
}