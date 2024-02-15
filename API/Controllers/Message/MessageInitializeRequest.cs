namespace ChatAppApi.Controllers.Message;

public class MessageInitializeRequest
{
    public int ToUserId { get; set; }
    public int FromUserId { get; set; }
}