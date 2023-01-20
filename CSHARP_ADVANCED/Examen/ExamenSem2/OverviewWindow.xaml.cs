using GameClubClassLibrary.DataAccess;
using GameClubClassLibrary.Entities;
using Shpielerij.FileManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ExamenSem2
{
    /// <summary>
    /// Interaction logic for OverviewWindow.xaml
    /// </summary>
    public partial class OverviewWindow : Window
    {
        public OverviewWindow(string bgCSVPath, string vgCSVPath)
        {
            InitializeComponent();
            BoardGameData.InitialiiseerBoardGameData(bgCSVPath);
            VideoGameData.InitialiiseerVideoGameData(vgCSVPath);
            DataGridBoardGames.ItemsSource = BoardGameData.GetBoardGameList();
            ListBoxVideoGames.ItemsSource = VideoGameData.GetVideoGameList();
            DataGridBoardGames.SelectedIndex = 0;
        }

        private void DataGridBoardGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BoardGame game = BoardGameData.BoardGameByIndex(DataGridBoardGames.SelectedIndex);
            ImageBoardGame.Source = new BitmapImage(new Uri($"../images/boardgames/{game.Source}", UriKind.Relative));
            TextBlockBoardGameAmazonPrice.Text = game.GetAmazonPrice().ToString();
            TextBlockBoardGameGeekGameShopPrice.Text = game.GetGeekGameShopPrice().ToString();
        }

        private void Top10_Button_Click(object sender, RoutedEventArgs e)
        {
            var query = BoardGameData.GetBoardGameList().OrderBy(x => x.Rank).
                Where(x => x.Rank <= 10);
            BoardGameData.LinqBoardGames = query.ToList();
            DataGridBoardGames.ItemsSource = query.ToList();
        }

        private void Post2015Filter_Button_Click(object sender, RoutedEventArgs e)
        {
            var query = BoardGameData.GetBoardGameList().
                OrderBy(x => x.ReleaseYear).
                Where(x => x.ReleaseYear > 2015);
            BoardGameData.LinqBoardGames = query.ToList();
            DataGridBoardGames.ItemsSource = query.ToList();
        }

        private void Under50_Button_Click(object sender, RoutedEventArgs e)
        {
            var query = BoardGameData.GetBoardGameList().
                OrderBy(x => x.GetAmazonPrice() > x.GetGeekGameShopPrice() ? x.GetAmazonPrice() : x.GetGeekGameShopPrice()).
                Where(x => x.GetAmazonPrice() < 50 || x.GetGeekGameShopPrice() < 50);
            BoardGameData.LinqBoardGames = query.ToList();
            DataGridBoardGames.ItemsSource = query.ToList();
        }

        private void ResetFilter_Button_Click(object sender, RoutedEventArgs e)
        {
            BoardGameData.LinqBoardGames = BoardGameData.GetBoardGameList();
            DataGridBoardGames.ItemsSource = BoardGameData.LinqBoardGames;
        }

        private void ListBoxVideoGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VideoGame game = VideoGameData.VideoGameByIndex(ListBoxVideoGames.SelectedIndex);
            TextBlockYear.Text = game.ReleaseYear.ToString();
            TextBlockGeekRating.Text = game.GeekRating.ToString();
            TextBlockAvgRating.Text = game.AverageRating.ToString();
            TextBlockNumberOfVoters.Text = game.NumberOfVoters.ToString();
            TextBlockGameMode.Text = game.IsSinglePlayerOnly ? "Singleplayer Only" : "Has Multiplayer";
            TextBlockBoardGameAmazonPrice.Text = game.GetAmazonPrice().ToString(); // De prijzen komen hier in de text property, maar verschijnen niet in het window??????
            TextBlockBoardGameGeekGameShopPrice.Text = game.GetGeekGameShopPrice().ToString();
            ImageVideoGame.Source = new BitmapImage(new Uri($"../images/videogames/{game.Source}", UriKind.Relative));
        }

        private void ExportXML_Button_Click(object sender, RoutedEventArgs e)
        {
            DataSet d = new DataSet();
            d.Tables.Add(BoardGameData.BoardGameDataTable);
            d.Tables.Add(VideoGameData.VideoGameDataTable);
            FileManagement.SaveDataSetAsXML(d);
        }
    }
}
