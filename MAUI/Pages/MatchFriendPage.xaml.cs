using ChatAppTutorial.ViewModels;

namespace ChatAppTutorial.Pages;

public partial class MatchFriendPage : ContentPage
{
    public MatchFriendPage()
    {
        InitializeComponent();
    }

    private void MatchFriendPage_OnNavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (this.BindingContext as MatchFriendPageViewModel).Initialize();

    }
}