using GameClubClassLibrary.DataAccess;
using Shpielerij.FileManagement;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExamenSem2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!UserData.IsValidLogin(TextBoxName.Text, TextBoxPassword.Password))
            {
                MessageBox.Show("Name or Password incorrect!", "Incorrect Login");
            }
            else
            {
                string bgCSVPath = string.Empty;
                string vgCSVPath = string.Empty;
                bool succes = false;
                while (!succes){
                    MessageBox.Show("Select the Board Game csv file", "Select CSV file");
                    bgCSVPath = FileManagement.FilePath();
                    if (File.ReadAllText(bgCSVPath).StartsWith("BoardGameId;Rank;ImageSource;" +
                    "Title;ReleaseYear;Description;GeekRating;" +
                    "AvgerageRating;NumVoters;DistributorPrice")) succes = true;
                    else MessageBox.Show("The incorrect file was chosen. Try again.");
                }

                succes = false;
                while (!succes)
                {
                    MessageBox.Show("Select the Video Game csv file", "Select CSV file");
                    vgCSVPath = FileManagement.FilePath();
                    if (File.ReadAllText(vgCSVPath).StartsWith("VideoGameId;Rank;ImageSource;Title;" +
                    "ReleaseYear;SingleplayerOnly;GeekRating;AverageRating;NumVoters")) succes = true;
                    else MessageBox.Show("The incorrect file was chosen. Try again.");
                }

                OverviewWindow window = new OverviewWindow(bgCSVPath, vgCSVPath);
                window.ShowDialog();
            }
        }
    }
}
