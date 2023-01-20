using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClubClassLibrary.Entities
{
    public class Game : IRetailable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Rank { get; set; }
        public string Source { get; set; }
        public int ReleaseYear { get; set; }
        public double GeekRating { get; set; }
        public double AverageRating { get; set; }
        public int NumberOfVoters { get; set; }

        public Game(int id, string title, int rank, string source, int releaseYear, 
            double geekRating, double averageRating, int numberOfVoters)
        {
            Id = id;
            Title = title;
            Rank = rank;
            Source = source;
            ReleaseYear = releaseYear;
            GeekRating = geekRating;
            AverageRating = averageRating;
            NumberOfVoters = numberOfVoters;
        }

        public Game() { }
        public virtual double GetAmazonPrice()
        {
            throw new NotImplementedException();
        }

        public virtual double GetGeekGameShopPrice()
        {
            throw new NotImplementedException();
        }
    }
}
