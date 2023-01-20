using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClubClassLibrary.Entities
{
    public class VideoGame : Game
    {

        public bool IsSinglePlayerOnly { get; set; }

        public VideoGame(int id, string title, int rank, string source, int releaseYear, double geekRating, double averageRating, int numberOfVoters) : base(id, title, rank, source, releaseYear, geekRating, averageRating, numberOfVoters)
        {
        }

        public VideoGame() { }

        public override double GetAmazonPrice()
        {
            double price = 0.0;
            if (ReleaseYear < 1990)
            {
                price = 9.99;
            }
            else if (ReleaseYear < 2010)
            {
                price = 59.99;
            }
            else
            {
                price = 69.99;
            }
            return Math.Round(price, 2);
        }

        public override double GetGeekGameShopPrice()
        {
            double price = 0.0;
            if (ReleaseYear < 2000)
            {
                price = 9.99;
            }
            else
            {
                price = 49.99;
            }
            return Math.Round(Rank <= 50 ? price * 1.5 : price, 2);
        }

        public override string ToString()
        {
            return $"{Title}";
        }
    }
}
