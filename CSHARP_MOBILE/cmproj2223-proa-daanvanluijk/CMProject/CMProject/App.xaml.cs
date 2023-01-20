using CMProject.Models;

namespace CMProject;

public partial class App : Application
{
	public bool rememberCredentials;
	public string userName;
	public string password;

	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
