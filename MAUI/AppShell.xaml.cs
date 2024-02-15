using ChatAppTutorial.Pages;

namespace ChatAppTutorial;

public partial class AppShell : Shell
{
	public AppShell(LoginPage loginPage)
	{
		InitializeComponent();
		
		Routing.RegisterRoute(nameof(LoginPage),typeof(LoginPage));
		Routing.RegisterRoute(nameof(ListChatPage),typeof(ListChatPage));
		Routing.RegisterRoute(nameof(ChatPage),typeof(ChatPage));
		Routing.RegisterRoute(nameof(MatchFriendPage),typeof(MatchFriendPage));
		Routing.RegisterRoute(nameof(PossibleFriendsPage), typeof(PossibleFriendsPage));
		
		this.CurrentItem = loginPage;
	}
}
