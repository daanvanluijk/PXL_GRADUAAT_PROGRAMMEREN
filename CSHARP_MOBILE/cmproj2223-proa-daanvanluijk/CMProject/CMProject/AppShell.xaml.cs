using CMProject.Views;

namespace CMProject;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(PagesView), typeof(PagesView));
        Routing.RegisterRoute(nameof(PageView), typeof(PageView));
		Routing.RegisterRoute(nameof(CloudView), typeof(CloudView));
		Routing.RegisterRoute(nameof(SettingsView), typeof(SettingsView));
    }
}
