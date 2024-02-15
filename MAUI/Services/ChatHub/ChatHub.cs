using Microsoft.AspNetCore.SignalR.Client;

namespace ChatAppTutorial.Services.ChatHub;

public class ChatHub
{
    private readonly HubConnection _hubConnection;

    private List<Action<int, string>> onReceiveMessageHandler;

    private ServiceProvider _serviceProvider;
    
    private static ChatHub _instance;

    public ChatHub()
    {
        _serviceProvider = ServiceProvider.GetInstance();

        _hubConnection = new HubConnectionBuilder()
            .WithUrl("http://10.0.2.2:5190" + "/ChatHub", options =>
            {
                options.Headers.Add("ChatHubBearer", _serviceProvider._accessToken);
                
            }).Build();
        
        onReceiveMessageHandler = new List<Action<int, string>>();

        _hubConnection.On<int, string>("ReceiveMessage", OnReceiveMessage);

    }

    public static ChatHub GetInstance()
    {
        return _instance ??= new ChatHub();
    }

    public async Task Connect()
    {
        await _hubConnection.StartAsync();
    }

    public async Task Disconnect()
    {
        await _hubConnection.StopAsync();
    }

    public async Task SendMessageToUser(int fromUserId, int toUserId, string message)
    {
        await _hubConnection.InvokeAsync("SendMessageToUser", fromUserId, toUserId, message);
    }

    public void AddReceivedMessageHandler(Action<int, string> handler)
    {
        onReceiveMessageHandler.Add(handler);
    }

    private void OnReceiveMessage(int userId, string message)
    {
        foreach (var handler in onReceiveMessageHandler)
        {
            handler(userId, message);
        }
    }
}