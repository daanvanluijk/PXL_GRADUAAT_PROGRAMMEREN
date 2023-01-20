using CMProject.Events;
using CMProject.Models;
using CMProject.Services;
using CMProject.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using Section = CMProject.Models.Section;

namespace CMProject.ViewModels
{
    public partial class PageViewModel : ObservableObject, INotifyPropertyChanged
    {
        public event EventHandler<SavedEventArgs> Saved;

        public UserPage Page
        {
            get => _userPageStorage.UserPages[_userPageStorage.currentPageIndex];
            set => SetProperty(_userPageStorage.UserPages[_userPageStorage.currentPageIndex], value, x => x = value);
        }

        [ObservableProperty]
        private bool _editing = false;

        [ObservableProperty]
        private int screenWidth;

        [ObservableProperty]
        private int screenHeight;

        [ObservableProperty]
        private int highlightedSectionNr = -1;

        private UserPageStorage _userPageStorage;
        private AudioService _audioService;

        public PageViewModel(UserPageStorage userPageStorage, AudioService audioService)
        {
            _userPageStorage = userPageStorage;
            _audioService = audioService;

            screenWidth = (int)(DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density);
            screenHeight = (int)(DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density);
        }

        public void UpdateCurrentPage()
        {
            OnPropertyChanged(nameof(Page));
        }

        [RelayCommand]
        private void SwitchEditMode()
        {
            _editing = !_editing;
        }

        [RelayCommand]
        private void AddParagraph()
        {
            _userPageStorage.UserPages[_userPageStorage.currentPageIndex].AddSection<Paragraph>();
            _audioService.PlaySound("paragraph");
            RefreshView();
        }

        [RelayCommand]
        private void AddImage()
        {
            _userPageStorage.UserPages[_userPageStorage.currentPageIndex].AddSection<ImageContent>();
            _audioService.PlaySound("image");
            RefreshView();
        }

        [RelayCommand]
        private void MoveSectionUp()
        {
            if (_userPageStorage.UserPages[_userPageStorage.currentPageIndex].MoveSectionUp(highlightedSectionNr))
            {
                ChangeHighlightedSection(highlightedSectionNr - 1);
                _audioService.PlaySound("move");
            }
            RefreshView();
        }

        [RelayCommand]
        private void MoveSectionDown()
        {
            if (_userPageStorage.UserPages[_userPageStorage.currentPageIndex].MoveSectionDown(highlightedSectionNr))
            {
                ChangeHighlightedSection(highlightedSectionNr + 1);
                _audioService.PlaySound("move");
            }
            RefreshView();
        }

        [RelayCommand]
        private void DeleteSection()
        {
            _userPageStorage.UserPages[_userPageStorage.currentPageIndex].DeleteSection(highlightedSectionNr);
            _audioService.PlaySound("delete");
            //if (!_userPageStorage.UserPages[_userPageStorage.currentPageIndex].Sections.Any())
            //{
            //    _userPageStorage.UserPages[_userPageStorage.currentPageIndex].AddSection<Paragraph>();
            //}
            RefreshView();
        }

        [RelayCommand]
        private void ChangeImage()
        {
            _userPageStorage.UserPages[_userPageStorage.currentPageIndex].ChangeImage(highlightedSectionNr);
            RefreshView();
        }

        [RelayCommand]
        private void SectionTapped(object section)
        {
            int newHighlightedSectionNr = _userPageStorage.UserPages[_userPageStorage.currentPageIndex].GetSectionIndex(section as Section);
            _userPageStorage.UserPages[_userPageStorage.currentPageIndex].HighlightSection(newHighlightedSectionNr, highlightedSectionNr);
            ChangeHighlightedSection(newHighlightedSectionNr);
        }

        [RelayCommand]
        private void SectionFocused(object section)
        {
            SectionTapped(section);
        }

        [RelayCommand]
        private void ParagraphTextChanged(object section)
        {
            if (((section as Section).Content as Paragraph).ShowContent == "")
                DeleteSection();
        }

        [RelayCommand]
        private void Appearing()
        {
            
        }

        [RelayCommand]
        private void Disappearing()
        {
            
        }

        [RelayCommand]
        private void Save()
        {
            _userPageStorage.SavePages();
            _audioService.PlaySound("save");
            Saved?.Invoke(this, new SavedEventArgs());
        }

        private void ChangeHighlightedSection(int sectionNr)
        {
            highlightedSectionNr = sectionNr;
            if (sectionNr < 0 || sectionNr >= _userPageStorage.UserPages[_userPageStorage.currentPageIndex].Sections.Count)
                return;
            //_page.DeleteEmptyParagraphs();
        }

        private async void RefreshView()
        {
            Page page = Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1];
            await Shell.Current.GoToAsync(nameof(PageView), false);
            Shell.Current.Navigation.RemovePage(page);
        }
    }
}