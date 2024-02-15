using ChatAppTutorial.ViewModels;

namespace ChatAppTutorial.Pages;

public partial class PossibleFriendsPage : ContentPage
{
    public PossibleFriendsPage()
    {
        InitializeComponent();
    }

    private void PossibleFriendsPage_OnNavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (this.BindingContext as PossibleFriendsPageViewModel).Initialize();

    }
}