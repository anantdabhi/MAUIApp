using System.Collections.ObjectModel;
using System.Text;
using System.Web;
using ChatAppTutorial.Models;
using ChatAppTutorial.Pages;
using ChatAppTutorial.Services.ListChat;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using ServiceProvider = ChatAppTutorial.Services.ServiceProvider;

namespace ChatAppTutorial.ViewModels;

public partial class PossibleFriendsPageViewModel : ObservableObject, IQueryAttributable
{
    public PossibleFriendsPageViewModel()
    {
        UserInfo = new User();
        PossibleFriends = new ObservableCollection<User>();
    }

    private async Task GetPossibleFriends()
    {
        var response = await ServiceProvider.GetInstance().CallWebApi<int, NonFriendInitializeResponse>
            ("/ListChat/GetPotentialFriends", HttpMethod.Post, UserInfo.Id);
        if (response.StatusCode == 200)
        {
            UserInfo = response.User;
            PossibleFriends = new ObservableCollection<User>(response.UserNonFriends);
        }
        else
        {
            await Shell.Current.DisplayAlert("ChatAppTest", response.StatusMessage, "OK");
        }
    }
    
    public void Initialize()
    {
        Task.Run(async () =>
        {
            IsProcessing = true;
            await GetPossibleFriends();
        }).GetAwaiter().OnCompleted(() => { IsProcessing = false; });
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || query.Count == 0) return;

        UserInfo.Id = int.Parse(HttpUtility.UrlDecode(query["userId"].ToString()) ?? string.Empty);
    }


    [ObservableProperty] private bool _isProcessing;
    [ObservableProperty] private User _userInfo;
    [ObservableProperty] private ObservableCollection<User> _possibleFriends;

    [RelayCommand]
    public async void GoToListPage()
    {
        IsProcessing = true;
        await Shell.Current.GoToAsync(nameof(ListChatPage) + $"?userId={8}");
        IsProcessing = false;
    }

    [RelayCommand]
    public async void AddFriend(int id)
    {
        User user = _possibleFriends.FirstOrDefault(x => x.Id == id);
        
        if (user == null) return;
        
        var confirm = Shell.Current.DisplayAlert("Confirmation", $"Added {user.UserName} as Friend", "Ok", "Cancel");
        
        if (!await confirm) return;

        IsProcessing = true;
        try
        {
            using (HttpClient client = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                httpRequestMessage.RequestUri = new Uri("http://10.0.2.2:5190/ListChat/AddFriend");
                httpRequestMessage.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserInfo.Id.ToString());

                var request = new AddFriendRequest
                {
                    UserId = UserInfo.Id.ToString(),
                    FriendId = id.ToString()
                };

                string jsonContent = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(jsonContent, encoding: Encoding.UTF8, "application/json");
                httpRequestMessage.Content = httpContent;

                await client.SendAsync(httpRequestMessage);
            }
        }
        catch (Exception)
        {
            return;
        }

        int index = _possibleFriends.IndexOf(user);

        _possibleFriends.RemoveAt(index);
        IsProcessing = false;
    }

    [RelayCommand]
    public async void DislikeFriend(int id)
    {
        User user = _possibleFriends.FirstOrDefault(x => x.Id == id);
        if (user == null) return;
        
        var confirm = Shell.Current.DisplayAlert("Confirmation", 
            $"You Disliked {user.UserName}", "Ok", "Cancel");

        if (!await confirm) return;

        int index = _possibleFriends.IndexOf(user);

        _possibleFriends.RemoveAt(index);
    }
    
}