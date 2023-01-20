using SQLite;
using System.Data.SqlTypes;

namespace CMProjectDataBase
{
    public class PagesSQLiteRepository
    {
        SQLiteConnection context;

        public User GetPages(User user)
        {
            Init();
            return context.Table<User>().FirstOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
        }

        public int SavePages(User user)
        {
            if (user.UserName is null || user.Password is null)
                return 0;
            Init();
            ClearPages(user);
            return context.Insert(user);
        }

        public int ClearPages(User user)
        {
            Init();
            return context.Table<User>().Delete(x => x.UserName == user.UserName && x.Password == user.Password);
        }

        private void Init()
        {
            if (context is not null)
            {
                return;
            }

            context = new SQLiteConnection(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserPages.db3"), SQLite.SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            CreateTableResult result = context.CreateTable<User>();
        }
    }
}
