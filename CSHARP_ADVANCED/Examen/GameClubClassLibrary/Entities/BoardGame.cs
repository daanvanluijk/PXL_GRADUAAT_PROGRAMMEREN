using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClubClassLibrary.Entities
{
    public class BoardGame : Game
    {

        public string Description { get; set; }
        public double DistributorPrice { get; set; }

        public BoardGame(int id, string title, int rank, string source, int releaseYear, double geekRating, double averageRating, int numberOfVoters) : base(id, title, rank, source, releaseYear, geekRating, averageRating, numberOfVoters)
        {
        }

        public BoardGame() { }

        public override double GetAmazonPrice()
        {
            return Math.Round(1.2 * DistributorPrice, 2);
        }

        public override double GetGeekGameShopPrice()
        {
            return Math.Round(Rank > 50 ? 1.12 * DistributorPrice : 1.25 * DistributorPrice, 2);
        }
    }
}
