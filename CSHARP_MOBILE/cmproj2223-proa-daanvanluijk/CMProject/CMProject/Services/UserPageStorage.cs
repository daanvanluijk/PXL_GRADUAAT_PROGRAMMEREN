using CMProject.Models;
using CMProject.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMProject.Services
{
    public class UserPageStorage
    {
        public List<UserPage> UserPages { get; set; }

        public int currentPageIndex;

        private PagesSQLiteRepository _pagesRepository;

        public UserPageStorage(PagesSQLiteRepository pagesSQLiteRepository)
        {
            _pagesRepository = pagesSQLiteRepository;
            RefreshPages();
        }

        public UserPageData[] GetPagesAsUserPageData()
        {
            return UserPages.Select(x => (UserPageData)x).ToArray();
        }

        public void RefreshPages()
        {
            List<UserPageData> userPageDatas = _pagesRepository.GetPages();
            UserPages = userPageDatas.Select(x => (UserPage)x).ToList();
        }

        public void SavePages()
        {
            _pagesRepository.SavePages(UserPages.Select(x => (UserPageData)x).ToList());
        }
    }
}
