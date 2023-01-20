using CMProject.Events;
using CMProject.ViewModels;

namespace CMProject.Views;

public partial class PagesView : ContentPage
{
	public PagesView(PagesViewModel pagesViewModel)
	{
		InitializeComponent();
		BindingContext = pagesViewModel;
        pagesViewModel.Saved += PagesViewModel_Saved;
    }

    private async void PagesViewModel_Saved(object sender, SavedEventArgs e)
    {
        await DisplayAlert("", "Pages were successfully saved!", "OK");
    }
}