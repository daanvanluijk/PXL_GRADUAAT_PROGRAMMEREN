using CMProject.Events;
using CMProject.Models;
using CMProject.ViewModels;

namespace CMProject.Views;

public partial class PageView : ContentPage
{
    public PageView(PageViewModel pageViewModel)
	{
		InitializeComponent();
		BindingContext = pageViewModel;
		pageViewModel.Saved += PageViewModel_Saved;
	}

	private async void PageViewModel_Saved(object sender, SavedEventArgs e)
	{
		await DisplayAlert("", "Pages were successfully saved!", "OK");
    }
}