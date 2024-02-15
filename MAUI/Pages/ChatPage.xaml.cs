using ChatAppTutorial.ViewModels;

namespace ChatAppTutorial.Pages;

public partial class ChatPage : ContentPage
{
    public ChatPage()
    {
        InitializeComponent();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (this.BindingContext as ChatPageViewModel).Initialize();
    }
}