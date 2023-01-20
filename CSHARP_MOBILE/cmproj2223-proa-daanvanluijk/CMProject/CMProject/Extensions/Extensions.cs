using CMProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMProject.Extensions
{
    public static class Extensions
    {
        public static IEnumerable<UserPage> SortBySearch(this IEnumerable<UserPage> userPages)
        {
            return userPages;
        }
    }
}
