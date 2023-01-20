using CMProject.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMProject.Repositories
{
    public class PagesSQLiteRepository
    {
        SQLiteConnection context;

        public List<UserPageData> GetPages()
        {
            Init();
            return context.Table<UserPageData>().ToList();
        }

        public int SavePages(List<UserPageData> itemsToSave)
        {
            Init();
            ClearPages();
            return context.InsertAll(itemsToSave);
        }

        public int SavePage(int index, UserPageData userPageData)
        {
            Init();
            List<UserPageData> list = GetPages();
            ClearPages();
            list[index] = userPageData;
            return context.InsertAll(list);
        }

        public int ClearPages()
        {
            Init();
            return context.Table<UserPageData>().Delete(x => true);
        }

        private void Init()
        {
            if (context is not null)
            {
                return;
            }

            context = new SQLiteConnection(Path.Combine(FileSystem.AppDataDirectory, "UserPages.db3"), SQLite.SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            CreateTableResult result = context.CreateTable<UserPageData>();
        }
    }
}
