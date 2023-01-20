using Shpielerij;
using Shpielerij.Disconnected;
using Shpielerij.FileManagement;
using Shpielerij.SQL;
using Shpielerij.SQL.Results;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TEST
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Settings.DataBaseName = "Spionshop";
            DataTable table = new DataTable();
            Klant[] klanten = new Klant[] { new Klant { Naam = "test" }, new Klant { Naam = "test2" } };
            Disconnected.AddColumnsToDataTable(table, typeof(Klant));
            Disconnected.AddObjectsAsRowsToDataTable(table, klanten);
        }

        public class Klant
        {
            public string Naam { get; set; }
        }
    }
}
