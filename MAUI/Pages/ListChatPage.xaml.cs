
using ChatAppTutorial.ViewModels;

namespace ChatAppTutorial.Pages;

public partial class ListChatPage : ContentPage
{
    public ListChatPage()
    {
        InitializeComponent();

    }

    private void ListChatPage_OnNavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (this.BindingContext as ListChatPageViewModel).Initialize();
    }
}