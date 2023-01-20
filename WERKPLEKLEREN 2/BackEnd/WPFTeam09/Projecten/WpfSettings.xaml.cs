using System;
using System.Collections.Generic;
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
using ClassLibTeam09.Settings;

namespace WPFTeam09.Projecten
{
    /// <summary>
    /// Interaction logic for WpfSettings.xaml
    /// </summary>
    public partial class WpfSettings : Window
    {
        
        public WpfSettings()
        {
            InitializeComponent();
        }

        private void window(object sender, RoutedEventArgs e)
        {
            txbSqlServer.Text = Settings.Database.Server;
            txbProjectDb.Text = Settings.Database.ProjectDatabase;
            txbIdividualtDb.Text = Settings.Database.IndividualDatabase;
            txbProjConnString.Text = Settings.Database.ProjectConnectionstring;
            txbIndConnString.Text = Settings.Database.IndividualConnectionstring;
        }
    }
}
