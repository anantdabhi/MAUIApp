using ChatAppApi.Entities;
using ChatAppApi.Function.User;
using Microsoft.EntityFrameworkCore;

namespace ChatAppApi.Function.Message;

public class MessageFunction : IMessageFunction
{
    private ChatAppContext _chatAppContext;
    private IUserFunction _userFunction;

    public MessageFunction(ChatAppContext chatAppContext, IUserFunction userFunction)
    {
        _chatAppContext = chatAppContext;
        _userFunction = userFunction;
    }

    public async Task<IEnumerable<LatestMessage>> GetLatestMessage(int userId)
    {
        var result = new List<LatestMessage>();

        var userFriends = await _chatAppContext.TblUserFriends
            .Where(friend => friend.UserId == userId)
            .ToListAsync();

        foreach (var friend in userFriends)
        {
            var lastMessage = await _chatAppContext.TblMessages
                .Where(message => (message.FromUserId == userId && message.ToUserId == friend.FriendId) ||
                                  message.ToUserId == userId && message.FromUserId == friend.FriendId)
                .OrderByDescending(message => message.Id)
                .FirstOrDefaultAsync();

            if (lastMessage != null)
            {
                result.Add(new LatestMessage
                {
                    UserId = userId,
                    Content = lastMessage.Content,
                    UserFriendInfo = _userFunction.GetUserById(friend.FriendId),
                    Id = lastMessage.Id,
                    IsRead = lastMessage.IsRead,
                    SendDateDay = lastMessage.SendDateDay,
                    SendDateTime = lastMessage.SendDateTime
                });
            }
        }

        return result;
    }

    public async Task<IEnumerable<Message>> GetMessages(int fromUserId, int toUserId)
    {
        var entities = await _chatAppContext.TblMessages
            .Where(x => (x.FromUserId == fromUserId && x.ToUserId == toUserId)
                        || (x.FromUserId == toUserId && x.ToUserId == fromUserId))
            .OrderBy(x => x.Id)
            .ToListAsync();

        return entities.Select(x => new Message
        {
            Id = x.Id,
            Content = x.Content,
            ToUserId = x.ToUserId,
            FromUserId = x.FromUserId,
            IsRead = x.IsRead,
            SendDateTime = x.SendDateTime,
            SendDateDay = x.SendDateDay
        });
    }

    public async Task<int> AddMessage(int fromUserId, int toUserId, string message)
    {
        string[] arr = DateTime.Now.ToShortDateString().Split("/");

        if (arr[0].Length == 1) arr[0] = "0" + arr[0];
        if (arr[1].Length == 1) arr[1] = "0" + arr[1];
        string sendDateDay = arr[0] + "/" + arr[1] + "/" + arr[2];

        var lastMessage = await _chatAppContext.TblMessages
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();
        
        var entity = new TblMessage
        {
            Id = lastMessage.Id + 1,
            FromUserId = fromUserId,
            ToUserId = toUserId,
            Content = message,
            SendDateDay = sendDateDay,
            SendDateTime = DateTime.Now.ToShortTimeString().Substring(0,5),
            IsRead = false
        };

        _chatAppContext.TblMessages.Add(entity);
        var result = await _chatAppContext.SaveChangesAsync();

        return result;
    }
}