using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClubClassLibrary.Entities
{
    public interface IRetailable
    {
        double GetAmazonPrice();
        double GetGeekGameShopPrice();
    }
}
