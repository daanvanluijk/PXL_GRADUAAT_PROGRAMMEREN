using CMProject.Models;
using CMProject.Services;
using CMProject.ViewModels;
using CommunityToolkit.Maui.Views;
using Newtonsoft.Json;

namespace CMProject.Views;

public partial class CloudView : ContentPage
{
	private UserPageStorage _userPageStorage;

	public CloudView(UserPageStorage userPageStorage, CloudViewModel cloudViewModel)
	{
        _userPageStorage = userPageStorage;
        InitializeComponent();
        BindingContext = cloudViewModel;
        cloudViewModel.CloudStored += CloudViewModel_CloudStored;
        cloudViewModel.CloudRetrieved += CloudViewModel_CloudRetrieved;
    }

    private async void CloudViewModel_CloudStored(object sender, Events.CloudStoredEventArgs e)
    {
        await DisplayAlert("", e.Message, "OK");
    }

    private async void CloudViewModel_CloudRetrieved(object sender, Events.CloudRetrievedEventArgs e)
    {
        await DisplayAlert("", e.Message, "OK");
    }

    /*private async void StoreTest()
	{
        HttpClient client = new HttpClient();
        User user = new User("Test", "Test");
        string pagesJson = JsonConvert.SerializeObject(_userPageStorage.GetPagesAsUserPageData().ToArray());
        user.UserPageDatas = pagesJson;
        string json = JsonConvert.SerializeObject(user);
        HttpResponseMessage response = await client.PostAsync("http://192.168.0.189:7777/Store", new StringContent(json));
    }

    private async void RetrieveTest()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"http://192.168.0.189:7777/Retrieve?UserName={"Test"}&Password={"Test"}");
        string content = await response.Content.ReadAsStringAsync();
    }*/
}