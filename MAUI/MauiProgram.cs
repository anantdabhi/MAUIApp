using ChatAppTutorial.Pages;
using ChatAppTutorial.Services.ChatHub;
using ChatAppTutorial.ViewModels;
using Microsoft.Extensions.Logging;

namespace ChatAppTutorial;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("MaterialIcons-Regular.ttf", "IconFontTypes");
			});

		builder.Services.AddSingleton<ChatHub>();
		
		builder.Services.AddSingleton<AppShell>();
		
		builder.Services.AddSingleton<LoginPage>();
		builder.Services.AddSingleton<ListChatPage>();
		builder.Services.AddSingleton<ChatPage>();
		builder.Services.AddSingleton<MatchFriendPage>();
		builder.Services.AddSingleton<PossibleFriendsPage>();
		
		builder.Services.AddSingleton<LoginPageViewModel>();
		builder.Services.AddSingleton<ListChatPageViewModel>();
		builder.Services.AddSingleton<ChatPageViewModel>();
		builder.Services.AddSingleton<MatchFriendPageViewModel>();
		builder.Services.AddSingleton<PossibleFriendsPageViewModel>();
		
#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
