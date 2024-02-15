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

public partial class MatchFriendPageViewModel : ObservableObject, IQueryAttributable
{
    public MatchFriendPageViewModel()
    {
        UserInfo = new User();
        _collection = new ObservableCollection<User>();
    }

    [ObservableProperty] private bool _isProcessing;
    [ObservableProperty] private string _avatarFile = "loki.png";
    [ObservableProperty] private string _userName = "a";
    [ObservableProperty] private string _hobbies = "b";
    [ObservableProperty] private string _age = "c";
    [ObservableProperty] private string _job = "d";
    [ObservableProperty] private bool _isOnline;
    [ObservableProperty] private bool _isAway;
    [ObservableProperty] private User _userInfo;
    [ObservableProperty] private bool _containsUser;
    [ObservableProperty] private bool _notContainsUser;
    private ObservableCollection<User> _collection;
    private int _index;
    private readonly Random _rnd = new();

    private void SetPotentialFriend()
    {
        AvatarFile = _collection[_index].AvatarFile;
        UserName = _collection[_index].UserName;
        Hobbies = _collection[_index].Hobbies;
        Age = _collection[_index].Age;
        Job = _collection[_index].Job;
        IsOnline = _collection[_index].IsOnline;
        IsAway = _collection[_index].IsAway;
    }

    private void GetDifferentPotentialFriend()
    {
        _collection.RemoveAt(_index);
        ContainsUser = _collection.Count != 0;
        NotContainsUser = !ContainsUser;
        if (NotContainsUser)
        {
            return;
        }

        _index = _rnd.Next(0, _collection.Count - 1);

        SetPotentialFriend();
    }

    async Task GetCollection()
    {
        var response = await ServiceProvider.GetInstance().CallWebApi<int, NonFriendInitializeResponse>
            ("/ListChat/GetNotFriends", HttpMethod.Post, UserInfo.Id);
        if (response.StatusCode == 200)
        {
            UserInfo = response.User;
            _collection = new ObservableCollection<User>(response.UserNonFriends);
        }
        else
        {
            await Shell.Current.DisplayAlert("ChatAppTest", response.StatusMessage, "OK");
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || query.Count == 0) return;

        UserInfo.Id = int.Parse(HttpUtility.UrlDecode(query["userId"].ToString()) ?? string.Empty);
    }

    public void Initialize()
    {
        Task.Run(async () =>
        {
            IsProcessing = true;
            await GetCollection();
            ContainsUser = _collection.Count != 0;
            NotContainsUser = !ContainsUser;
            if (NotContainsUser)
            {
                return;
            }

            _index = _rnd.Next(0, _collection.Count - 1);

            SetPotentialFriend();
        }).GetAwaiter().OnCompleted(() => { IsProcessing = false; });
    }

    [RelayCommand]
    public async void AddFriend()
    {
        if (_collection.Count == 0) return;

        var confirm = Shell.Current.DisplayAlert("Swiped!", $"Added {UserName} as Friend", "Ok", "Cancel");

        if (!await confirm) return;

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
                    FriendId = _collection[_index].Id.ToString()
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

        GetDifferentPotentialFriend();
    }

    [RelayCommand]
    public async void Dislike()
    {
        if (_collection.Count == 0) return;


        var confirm = Shell.Current.DisplayAlert("Swiped!", $"You Disliked {UserName}", "Ok", "Cancel");

        if (!await confirm) return;
        GetDifferentPotentialFriend();
    }
    
    [RelayCommand]
    public async void GoToListPage()
    {
        IsProcessing = true;
        await Shell.Current.GoToAsync(nameof(ListChatPage) + $"?userId={UserInfo.Id}");
        IsProcessing = false;
    }
}