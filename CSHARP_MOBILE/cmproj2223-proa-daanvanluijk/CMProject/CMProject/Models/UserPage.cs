using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace CMProject.Models
{
    public partial class UserPage : ObservableObject
    {
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private List<Section> _sections;

        [ObservableProperty]
        private bool currentAndHighlightedPageNrMatch = false;

        public UserPage(string title)
        {
            _sections = new List<Section>();
            this.title = title;
        }

        public UserPage() : this("test")
        {
        }

        public int GetSectionIndex(Section section)
        {
            return _sections.FindIndex(x => x == section);
        }

        public void AddSectionAfter<T>(Section section) where T : SectionContent, new()
        {
            int index = _sections.FindIndex(x => x == section);
            AddSectionAfter<T>(index);
        }

        public void AddSectionAfter<T>(int sectionNr) where T : SectionContent, new()
        {
            Section newSection = new Section();
            newSection.Content = new T();
            _sections.Insert(sectionNr, newSection);
        }

        public void AddSection<T>() where T : SectionContent, new()
        {
            Section newSection = new Section();
            newSection.Content = new T();
            _sections.Add(newSection);
        }

        public void SetContent(uint sectionNr, SectionContent content)
        {
            Section section = _sections[(int)sectionNr];
            section.Content = content;
        }

        public bool MoveSectionUp(Section section)
        {
            int sectionNr = _sections.FindIndex(x => x == section);
            if (sectionNr > 0)
            {
                Section section2 = _sections[sectionNr - 1];
                _sections[sectionNr - 1] = section;
                _sections[sectionNr] = section2;
                return true;
            }
            return false;
        }

        public bool MoveSectionUp(int sectionNr)
        {
            if (sectionNr < 0 || sectionNr >= _sections.Count)
                return false;
            return MoveSectionUp(_sections[sectionNr]);
        }

        public bool MoveSectionDown(Section section)
        {
            int sectionNr = _sections.FindIndex(x => x == section);
            if (sectionNr < _sections.Count - 1)
            {
                Section section2 = _sections[sectionNr + 1];
                _sections[sectionNr + 1] = section;
                _sections[sectionNr] = section2;
                return true;
            }
            return false;
        }

        public bool MoveSectionDown(int sectionNr)
        {
            if (sectionNr < 0 || sectionNr >= _sections.Count)
                return false;
            return MoveSectionDown(_sections[sectionNr]);
        }

        public void DeleteSection(Section section)
        {
            _sections.Remove(section);
        }

        public void DeleteSection(int sectionNr)
        {
            if (sectionNr < 0 || sectionNr >= _sections.Count)
                return;
            DeleteSection(_sections[sectionNr]);
        }

        public void DeleteEmptyParagraphs()
        {
            List<int> sectionNrs = _sections
                .Where(x => x.Content is Paragraph && string.IsNullOrWhiteSpace((x.Content as Paragraph).ShowContent))
                .Select(x => _sections.FindIndex(y => y == x))
                .ToList();
            foreach (int sectionNr in sectionNrs)
            {
                DeleteSection(sectionNr);
            }
        }

        public async void ChangeImage(Section section)
        {
            FileResult result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Choose an Image",
                FileTypes = FilePickerFileType.Images,
            });
            if (result == null)
                return;

            byte[] bytes = await File.ReadAllBytesAsync(result.FullPath);
            string filePath = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
            await File.WriteAllBytesAsync(filePath, bytes);

            (section.Content as ImageContent).ShowContent = filePath;
        }

        public void ChangeImage(int sectionNr)
        {
            if (sectionNr < 0 || sectionNr >= _sections.Count)
                return;
            ChangeImage(_sections[sectionNr]);
        }

        public void HighlightSection(int currentSectionNr, int previousSectionNr)
        {
            if (currentSectionNr >= 0 && currentSectionNr < _sections.Count)
                _sections[currentSectionNr].CurrentAndHighlightedSectionNrMatch = true;
            if (previousSectionNr >= 0 && previousSectionNr < _sections.Count && previousSectionNr != currentSectionNr)
                _sections[previousSectionNr].CurrentAndHighlightedSectionNrMatch = false;
        }

        public static explicit operator UserPage(UserPageData x)
        {
            UserPage userPage = new UserPage(x.Title);
            List<Section> sections = new List<Section>();
            List<Dictionary<string, string>> json = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(x.Sections);
            for (int i = 0; i < json.Count; i++)
            {
                Dictionary<string, string> sectionJson = json[i];
                Section section = new Section();
                SectionContent sectionContent;
                switch (sectionJson["SectionContentType"])
                {
                    case nameof(Paragraph):
                        sectionContent = new Paragraph(sectionJson["Content"]);
                        break;
                    case nameof(ImageContent):
                        sectionContent = new ImageContent();
                        (sectionContent as ImageContent).ShowContent = sectionJson["Content"];
                        break;
                    default:
                        throw new Exception("Geef het op maat");
                }
                section.Content = sectionContent;
                sections.Add(section);
            }
            userPage.Sections = sections;
            return userPage;
        }

        public static explicit operator UserPageData(UserPage x)
        {
            UserPageData userPageData = new UserPageData();
            userPageData.Title = x.title;
            List<Dictionary<string, string>> json = new List<Dictionary<string, string>>();
            for (int i = 0; i < x.Sections.Count; i++)
            {
                Dictionary<string, string> sectionJson = new Dictionary<string, string>();
                Section section = x.Sections[i];
                Type sectionContentType = section.Content.GetType();
                sectionJson["SectionContentType"] = sectionContentType.Name;
                switch (sectionContentType.Name)
                {
                    case nameof(Paragraph):
                        sectionJson["Content"] = (section.Content as Paragraph).ShowContent;
                        break;
                    case nameof(ImageContent):
                        string file = (section.Content as ImageContent).ShowContent.ToString().Substring(6);
                        sectionJson["Content"] = file;
                        break;
                    default:
                        throw new Exception("Geef het op maat");
                }
                json.Add(sectionJson);
            }
            userPageData.Sections = JsonConvert.SerializeObject(json);
            return userPageData;
        }
    }
}
