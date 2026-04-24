using DietApp.Services;
using DietApp.ViewModels;
using DietApp.Views;
using Microsoft.Extensions.Logging;

namespace DietApp;

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
			});

		builder.Services.AddSingleton<DataService>();

		builder.Services.AddSingleton<GoalViewModel>();
		builder.Services.AddSingleton<ProgressViewModel>();

		builder.Services.AddSingleton<GoalPage>();
		builder.Services.AddSingleton<ProgressPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
