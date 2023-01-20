using CMProject.Events;
using CMProject.Models;
using CMProject.Services;
using CMProject.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMProject.ViewModels
{
    public partial class CloudViewModel : ObservableObject
    {
        public event EventHandler<CloudStoredEventArgs> CloudStored;
        public event EventHandler<CloudRetrievedEventArgs> CloudRetrieved;

        public string Username
        {
            get => username;
            set
            {
                SetProperty(ref username, value);
                if (Preferences.Get(nameof(App.rememberCredentials), false) == true)
                {
                    Preferences.Set(nameof(App.userName), value);
                }
            }
        }
        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
                if (Preferences.Get(nameof(App.rememberCredentials), false))
                {
                    Preferences.Set(nameof(App.password), value);
                }
            }
        }

        private string username;
        private string password;

        private UserPageStorage _userPageStorage;
        private AudioService _audioService;

        public CloudViewModel(UserPageStorage userPageStorage, AudioService audioService)
        {
            _userPageStorage = userPageStorage;
            _audioService = audioService;
        }

        [RelayCommand]
        private async void Store()
        {
            LoadingPopup loadingPopup = new LoadingPopup();
            Application.Current.MainPage.ShowPopup(loadingPopup);
            if (!Validate())
            {
                loadingPopup.Close();
                CloudStored?.Invoke(this, new CloudStoredEventArgs("Please enter a username and password!"));
                return;
            }
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(2000);
            User user = new User(username, password);
            string pagesJson = JsonConvert.SerializeObject(_userPageStorage.GetPagesAsUserPageData().ToArray());
            user.UserPageDatas = pagesJson;
            string json = JsonConvert.SerializeObject(user);
            try
            {
                HttpResponseMessage response = await client.PostAsync("http://192.168.0.189:7777/Store", new StringContent(json));
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
                _audioService.PlaySound("upload");
                loadingPopup.Close();
                CloudStored?.Invoke(this, new CloudStoredEventArgs("Successfully stored user data to the cloud!"));
            }
            catch (Exception)
            {
                loadingPopup.Close();
                CloudStored?.Invoke(this, new CloudStoredEventArgs("Something went wrong while storing user data to the cloud!"));
            }
        }

        [RelayCommand]
        private async void Retrieve()
        {
            LoadingPopup loadingPopup = new LoadingPopup();
            Application.Current.MainPage.ShowPopup(loadingPopup);
            if (!Validate())
            {
                loadingPopup.Close();
                CloudRetrieved?.Invoke(this, new CloudRetrievedEventArgs("Please enter a username and password!"));
                return;
            }
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(2000);
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://192.168.0.189:7777/Retrieve?UserName={username}&Password={password}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
                _audioService.PlaySound("download");
                loadingPopup.Close();
                CloudRetrieved?.Invoke(this, new CloudRetrievedEventArgs("Successfully retrieved user data from the cloud!"));
                string content = await response.Content.ReadAsStringAsync();
                User user = JsonConvert.DeserializeObject<User>(content);
                UserPageData[] userPageDatas = JsonConvert.DeserializeObject<UserPageData[]>(user.UserPageDatas);
                _userPageStorage.UserPages = userPageDatas.Select(x => (UserPage)x).ToList();
                _userPageStorage.SavePages();
            }
            catch (Exception)
            {
                loadingPopup.Close();
                CloudRetrieved?.Invoke(this, new CloudRetrievedEventArgs("Something went wrong while retrieving user data from the cloud!"));
            }
        }

        [RelayCommand]
        private void Appearing()
        {
            if (Preferences.Get(nameof(App.rememberCredentials), false))
            {
                username = Preferences.Get(nameof(App.userName), "");
                password = Preferences.Get(nameof(App.password), "");
                OnPropertyChanged(nameof(Username));
                OnPropertyChanged(nameof(Password));
            }
            else
            {
                username = "";
                password = "";
                OnPropertyChanged(nameof(Username));
                OnPropertyChanged(nameof(Password));
            }
        }

        private bool Validate()
        {
            return username is not null && password is not null;
        }
    }
}
