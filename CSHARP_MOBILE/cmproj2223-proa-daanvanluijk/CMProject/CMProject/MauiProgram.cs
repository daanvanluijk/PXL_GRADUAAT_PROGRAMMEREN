using CMProject.Repositories;
using CMProject.Services;
using CMProject.ViewModels;
using CMProject.Views;
using CommunityToolkit.Maui;
using Plugin.Maui.Audio;

namespace CMProject;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("RobotoSlab-Black.tff", "RobotoSlabBlack");
                fonts.AddFont("RobotoSlab-Bold.tff", "RobotoSlabBold");
                fonts.AddFont("RobotoSlab-ExtraBold.tff", "RobotoSlabExtraBold");
                fonts.AddFont("RobotoSlab-ExtraLight.tff", "RobotoSlabExtraLight");
                fonts.AddFont("RobotoSlab-Light.tff", "RobotoSlabLight");
                fonts.AddFont("RobotoSlab-Medium.tff", "RobotoSlabMedium");
                fonts.AddFont("RobotoSlab-Regular.tff", "RobotoSlabRegular");
                fonts.AddFont("RobotoSlab-SemiBold.tff", "RobotoSlabSemiBold");
                fonts.AddFont("RobotoSlab-Thin.tff", "RobotoSlabThin");
            });

		builder.Services.AddSingleton<PagesSQLiteRepository>();
		builder.Services.AddSingleton<HomeView>();
		builder.Services.AddSingleton<HomeViewModel>();
		builder.Services.AddSingleton<PagesView>();
		builder.Services.AddSingleton<PagesViewModel>();
		builder.Services.AddSingleton<PageView>();
		builder.Services.AddSingleton<PageViewModel>();
		builder.Services.AddSingleton<UserPageStorage>();
		builder.Services.AddSingleton<CloudView>();
		builder.Services.AddSingleton<CloudViewModel>();
		builder.Services.AddSingleton<SettingsView>();
		builder.Services.AddSingleton<SettingsViewModel>();


		// Audio stuff
        builder.Services.AddSingleton(AudioManager.Current);
		builder.Services.AddSingleton<AudioService>();

        return builder.Build();
	}
}
