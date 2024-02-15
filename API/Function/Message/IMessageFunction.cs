namespace ChatAppApi.Function.Message;

public interface IMessageFunction
{
    Task<IEnumerable<LatestMessage>> GetLatestMessage(int userId);

    Task<IEnumerable<Message>> GetMessages(int fromUserId, int toUserId);

    Task<int> AddMessage(int fromUserId, int toUserId, string message);
}