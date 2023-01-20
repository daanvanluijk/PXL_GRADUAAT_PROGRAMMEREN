using CMProject.Models;
using CMProject.Services;
using CMProject.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMProject.ViewModels
{
    public partial class HomeViewModel
    {
        [RelayCommand]
        public async void GoToPagesView()
        {
            await Shell.Current.GoToAsync(nameof(PagesView));
        }

        [RelayCommand]
        public async void GoToCloudView()
        {
            await Shell.Current.GoToAsync(nameof(CloudView));
        }

        [RelayCommand]
        public async void GoToSettingsView()
        {
            await Shell.Current.GoToAsync(nameof(SettingsView));
        }
    }
}
