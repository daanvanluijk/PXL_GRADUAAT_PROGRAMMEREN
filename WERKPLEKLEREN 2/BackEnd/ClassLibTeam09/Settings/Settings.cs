using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam09.Settings
{
    public static class Settings
    {

        public static class Database
        {

            private static string server = @"server = LAPTOP-RCNUI3OO\SQLEXPRESS";
            private static string pxlServer = @"server = 10.128.4.7";
            private static string pxlDatabase = @"database = pxl2022Team09";
            private static string pxlUser = "User Id = pxluser2022";
            private static string pxlPwd = "Password = pxluser2022";
            private static string pxlConnString = $"{pxlServer};{pxlDatabase};{pxlUser};{pxlPwd}";
            private static string projectDb = @"Database=pxl2022Team09";
            private static string individualDb = @"Database=pxl2022Team09";
            private static string security = "Integrated security = true";
            private static string projConnString = $"{server};{projectDb};{security}";
            private static string indConnString = $"{server};{individualDb};{security}";
            public static string Server => server;
            public static string PxlServer => pxlServer;
            public static string ProjectDatabase => projectDb;
            public static string PxlDatabase => pxlDatabase;
            public static string IndividualDatabase => projectDb;
            public static string Security => security;
            public static string ProjectConnectionstring => projConnString;
            public static string IndividualConnectionstring => indConnString;
            public static string PxlConnectionString => pxlConnString;
            public static string ConnectionString => pxlConnString;
        }
       
    }
}
