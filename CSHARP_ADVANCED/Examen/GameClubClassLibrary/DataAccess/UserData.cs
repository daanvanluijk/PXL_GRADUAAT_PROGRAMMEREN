using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClubClassLibrary.DataAccess
{
    public static class UserData
    {
        private static List<string> adminNames = new List<string>()
        {
            "Koen",
            "Kristof",
            "Sander",
        };
        private static string password = "PXL123";

        public static bool IsValidLogin(string userName, string _password)
        {
            return adminNames.Contains(userName) && _password == password ? true : false;
        }
    }
}
