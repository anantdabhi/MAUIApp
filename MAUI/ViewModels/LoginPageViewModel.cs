using ChatAppTutorial.Pages;
using ChatAppTutorial.Services.Authenticate;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiceProvider = ChatAppTutorial.Services.ServiceProvider;

namespace ChatAppTutorial.ViewModels;

public partial class LoginPageViewModel : ObservableObject
{
    public LoginPageViewModel()
    {
        UserName = "";
        Password = "";
        IsProcessing = false;
    }

    [ObservableProperty] private string _userName;
    [ObservableProperty] private string _password;
    [ObservableProperty] private bool _isProcessing;

    [RelayCommand]
    async Task Login()
    {
        if (IsProcessing) return;

        if (UserName.Trim() == "" || Password.Trim() == "") return;

        IsProcessing = true;

        try
        {
            var request = new AuthenticateRequest
            {
                LoginId = UserName,
                Password = Password
            };

            var response = await ServiceProvider.GetInstance().Authenticate(request);
            if (response.StatusCode == 200)
            {
                await Shell.Current.GoToAsync(nameof(ListChatPage) + $"?userId={response.Id}");
            }
            else
            {
                await Shell.Current.DisplayAlert("ChatApp Error Occurred", response.StatusMessage, "OK");
            }
        }
        catch (Exception e)
        {
            await Shell.Current.DisplayAlert("ChatApp", e.Message, "OK");

        }
        

        IsProcessing = false;
    }
}