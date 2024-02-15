using System.Collections.ObjectModel;
using System.Web;
using ChatAppTutorial.Models;
using ChatAppTutorial.Pages;
using ChatAppTutorial.Services.ChatHub;
using ChatAppTutorial.Services.ListChat;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiceProvider = ChatAppTutorial.Services.ServiceProvider;

namespace ChatAppTutorial.ViewModels;

public partial class ListChatPageViewModel : ObservableObject, IQueryAttributable
{
    private ChatHub _chatHub;

    public ListChatPageViewModel()
    {
        UserInfo = new User();
        UserFriends = new ObservableCollection<User>();
        LatestMessages = new ObservableCollection<LatestMessage>();

        _chatHub = ChatHub.GetInstance();
        _chatHub.Connect();
        _chatHub.AddReceivedMessageHandler(OnReceivedMessage);

        MessagingCenter.Send<string>("StartService", "MessageForegroundService");
        MessagingCenter.Send<string, string[]>("StartService", "MessageNotificationService",
            new string[] { });
    }

    [ObservableProperty] private User _userInfo;
    [ObservableProperty] private ObservableCollection<User> _userFriends;
    [ObservableProperty] private ObservableCollection<LatestMessage> _latestMessages;
    [ObservableProperty] private bool _isProcessing;
    [ObservableProperty] private bool _hasPossibleFriend = false;
    [ObservableProperty] private bool _hasNotPossibleFriend = true;

    [RelayCommand]
    async Task GetListFriends()
    {
        var response = await ServiceProvider.GetInstance().CallWebApi<int, ListChatInitialResponse>
            ("/ListChat/Initialize", HttpMethod.Post, UserInfo.Id);
        if (response.StatusCode == 200)
        {
            UserInfo = response.User;
            UserFriends = new ObservableCollection<User>(response.UserFriends);
            LatestMessages = new ObservableCollection<LatestMessage>(response.LatestMessages);
            HasPossibleFriend = response.HasPossibleFriend;
            HasNotPossibleFriend = !HasPossibleFriend;
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
            await GetListFriends();
        }).GetAwaiter().OnCompleted(() => { IsProcessing = false; });
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || query.Count == 0) return;

        UserInfo.Id = int.Parse(HttpUtility.UrlDecode(query["userId"].ToString()) ?? string.Empty);
    }

    void OnReceivedMessage(int fromUserId, string message)
    {
        var latestMessage = LatestMessages.FirstOrDefault(x => x.UserFriendInfo.Id == fromUserId);
        if (latestMessage != null)
        {
            LatestMessages.Remove(latestMessage);
        }

        var newLatestMessage = new LatestMessage
        {
            UserId = UserInfo.Id,
            Content = message,
            UserFriendInfo = UserFriends.FirstOrDefault(x => x.Id == fromUserId)
        };

        LatestMessages.Insert(0, newLatestMessage);

        OnPropertyChanged("LatestMessage");

        MessagingCenter.Send<string, string[]>("Notify", "MessageNotificationService",
            new string[] { newLatestMessage.UserFriendInfo.UserName, newLatestMessage.Content });
    }

    [RelayCommand]
    public async void OpenChatPage(int param)
    {
        IsProcessing = true;
        await Shell.Current.GoToAsync($"{nameof(ChatPage)}?fromUserId={UserInfo.Id}&toUserId={param}");
        IsProcessing = false;
    }

    [RelayCommand]
    public async void OpenAddFriendPage()
    {
        IsProcessing = true;
        await Shell.Current.GoToAsync(nameof(MatchFriendPage) + $"?userId={UserInfo.Id}");
        IsProcessing = false;
    }

    [RelayCommand]
    public async void OpenAddPossibleFriendsPage()
    {
        IsProcessing = true;
        await Shell.Current.GoToAsync(nameof(PossibleFriendsPage) + $"?userId={UserInfo.Id}");
        IsProcessing = false;
    }
}