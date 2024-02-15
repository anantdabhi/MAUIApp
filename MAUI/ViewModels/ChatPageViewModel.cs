using System.Collections.ObjectModel;
using System.Web;
using ChatAppTutorial.Models;
using ChatAppTutorial.Pages;
using ChatAppTutorial.Services.ChatHub;
using ChatAppTutorial.Services.Message;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiceProvider = ChatAppTutorial.Services.ServiceProvider;

namespace ChatAppTutorial.ViewModels;

public partial class ChatPageViewModel : ObservableObject, IQueryAttributable
{
    private ChatHub _chatHub;
    
    public ChatPageViewModel()
    {
        Messages = new ObservableCollection<Message>();

        _chatHub = ChatHub.GetInstance();
        _chatHub.AddReceivedMessageHandler(OnReceiveMessage);
        _chatHub.Connect();
    }


    async Task GetMessages()
    {
        var request = new MessageInitializeRequest()
        {
            FromUserId = FromUserId,
            ToUserId = ToUserId,
        };
        var response = await ServiceProvider.GetInstance()
            .CallWebApi<MessageInitializeRequest, MessageInitializeResponse>
                ("/Message/Initialize", HttpMethod.Post, request);

        if (response.StatusCode == 200)
        {
            FriendInfo = response.FriendInfo;
            Messages = new ObservableCollection<Message>(response.Messages);
        }
        else
        {
            await Shell.Current.DisplayAlert("ChatApp", response.StatusMessage, "OK");
        }
    }

    public void Initialize()
    {
        Task.Run(async () =>
        {
            IsProcessing = true;
            await GetMessages();
        }).GetAwaiter().OnCompleted(() => { IsProcessing = false; });
    }

    private void OnReceiveMessage(int fromUserId, string message)
    {
        string[] arr = DateTime.Now.ToShortDateString().Split("/");

        if (arr[0].Length == 1) arr[0] = "0" + arr[0];
        if (arr[1].Length == 1) arr[1] = "0" + arr[1];
        string sendDateDay = arr[0] + "/" + arr[1] + "/" + arr[2];
                
        Messages.Add(new Message
        {
            Content = Message,
            FromUserId = ToUserId,
            ToUserId = fromUserId,
            SendDateTime = DateTime.Now.ToShortTimeString().Substring(0,5),
            SendDateDay = sendDateDay,
            IsRead = false
        });
    }

    [ObservableProperty] private int _fromUserId;
    [ObservableProperty] private int _toUserId;
    [ObservableProperty] private User _friendInfo;
    [ObservableProperty] private ObservableCollection<Message> _messages;
    [ObservableProperty] private bool _isProcessing;
    [ObservableProperty] private string _message;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || query.Count == 0) return;

        FromUserId = int.Parse(HttpUtility.UrlDecode(query["fromUserId"].ToString()));
        ToUserId = int.Parse(HttpUtility.UrlDecode(query["toUserId"].ToString()));
    }

    [RelayCommand]
    public async void SendMessage()
    {
        try
        {
            if (Message.Trim() != "")
            {
                await _chatHub.SendMessageToUser(FromUserId, ToUserId, Message);

                string[] arr = DateTime.Now.ToShortDateString().Split("/");

                if (arr[0].Length == 1) arr[0] = "0" + arr[0];
                if (arr[1].Length == 1) arr[1] = "0" + arr[1];
                string sendDateDay = arr[0] + "/" + arr[1] + "/" + arr[2];
                
                Messages.Add(new Message
                {
                    Content = Message,
                    FromUserId = FromUserId,
                    ToUserId = ToUserId,
                    SendDateTime = DateTime.Now.ToShortTimeString().Substring(0,5),
                    SendDateDay = sendDateDay,
                    IsRead = false
                });

                Message = "";
            }
        }
        catch (Exception e)
        {
            await Shell.Current.DisplayAlert("ChatApp", e.Message, "OK");
        }
    }

    [RelayCommand]
    public async void GoBackListPage()
    {
        IsProcessing = true;
        await Shell.Current.GoToAsync(nameof(ListChatPage) + $"?userId={_fromUserId.ToString()}");
        IsProcessing = false;
    }
}