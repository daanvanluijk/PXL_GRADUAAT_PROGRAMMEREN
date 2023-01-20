using CMProject.Events;
using CMProject.Extensions;
using CMProject.Models;
using CMProject.Services;
using CMProject.Views;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace CMProject.ViewModels
{
    public partial class PagesViewModel : ObservableObject
    {
        public event EventHandler<SavedEventArgs> Saved;

        public ObservableCollection<UserPage> UserPages
        {
            get
            {
                return !string.IsNullOrWhiteSpace(search)
                ? _userPageStorage.UserPages
                    .Where(x => x.Title.ToLower().Contains(search.ToLower()) || x.Sections.Any(y => y.Content is Paragraph && (y.Content as Paragraph).ShowContent.ToLower().Contains(search.ToLower())))
                    .OrderBy(x =>
                    {
                        bool titleContains = x.Title.ToLower().Contains(search.ToLower());
                        if (titleContains)
                        {
                            return x.Title;
                        }
                        return "Z" + x.Title;
                    })
                    .ToObservableCollection()
                : _userPageStorage.UserPages
                    .ToObservableCollection();
            }  
        }

        public string Search
        {
            get => search;
            set
            {
                SetProperty(ref search, value);
                OnPropertyChanged(nameof(UserPages));
            }
        }

        private string search = "";

        /*public List<UserPage> Pages
        {
            get => _userPageStorage.UserPages;
            set => SetProperty(_userPageStorage.UserPages, value, x => x = value);
        }*/

        [ObservableProperty]
        private int highlightedPageNr = -1;

        private UserPageStorage _userPageStorage;
        private PageViewModel _pageViewModel;
        private AudioService _audioService;

        public PagesViewModel(
            UserPageStorage userPageStorage, 
            PageViewModel pageViewModel,
            AudioService audioService
        )
        {
            _userPageStorage = userPageStorage;
            _pageViewModel = pageViewModel;
            _audioService = audioService;
        }

        [RelayCommand]
        public async void GoToPageView(object page)
        {
            _userPageStorage.currentPageIndex = _userPageStorage.UserPages.IndexOf(page as UserPage);
            _pageViewModel.UpdateCurrentPage();
            _audioService.PlaySound("pencil");
            await Shell.Current.GoToAsync(nameof(PageView));
        }

        [RelayCommand]
        public void AddPage()
        {
            _userPageStorage.UserPages.Add(new UserPage("Page " + _userPageStorage.UserPages.Count));
            _audioService.PlaySound("add_page");
            OnPropertyChanged(nameof(UserPages));
        }

        [RelayCommand]
        public void DeletePage()
        {
            _userPageStorage.UserPages.RemoveAt(highlightedPageNr);
            _audioService.PlaySound("delete");
            highlightedPageNr = -1;
            OnPropertyChanged(nameof(UserPages));
        }

        [RelayCommand]
        public void PageTapped(object page)
        {
            if (highlightedPageNr != -1)
            {
                _userPageStorage.UserPages[highlightedPageNr].CurrentAndHighlightedPageNrMatch = false;
            }
            highlightedPageNr = _userPageStorage.UserPages.FindIndex(x => x == page as UserPage);
            if (highlightedPageNr == -1)
            {
                return;
            }
            (page as UserPage).CurrentAndHighlightedPageNrMatch = true;
        }

        [RelayCommand]
        public void Appearing()
        {
            OnPropertyChanged(nameof(UserPages));
        }

        [RelayCommand]
        public void Save()
        {
            _userPageStorage.SavePages();
            _audioService.PlaySound("save");
            Saved?.Invoke(this, new SavedEventArgs());
        }
    }
}
