using CMProject.ViewModels;

namespace CMProject.Views;

public partial class HomeView : ContentPage
{
	public HomeView(HomeViewModel homeViewModel)
	{
		InitializeComponent();
		BindingContext = homeViewModel;
    }
}