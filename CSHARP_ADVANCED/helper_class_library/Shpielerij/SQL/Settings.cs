using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shpielerij.SQL
{
    public static class Settings
    {
        public static string DataBaseName = "";
        public static string DataSource = @"LOCALHOST\SQLEXPRESS";
        public static string ConnectionString {
            get
            {
                if (DataBaseName != "")
                    return $@"Initial Catalog = {DataBaseName}; Data Source = {DataSource}; Integrated Security = true";
                throw new Exception("Settings DataBaseName is niet gedefiniëerd!");
            }
        }
    }
}
