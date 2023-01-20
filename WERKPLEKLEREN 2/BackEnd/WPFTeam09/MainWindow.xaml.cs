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


namespace WPFTeam09
{
    /// <summary>
    /// Interaction logic for MainWIndow.xaml
    /// </summary>
    public partial class MainWIndow : Window
    {
        public MainWIndow()
        {
            InitializeComponent();
        }

        private void mntSettings_Click(object sender, RoutedEventArgs e)
        {
            Projecten.WpfSettings w = new Projecten.WpfSettings();
            w.Show();
        }

        private void btn_Stef_Wouters(object sender, RoutedEventArgs e)
        {
            Induviduele_opdrachten.WpfStefWouters wpf = new Induviduele_opdrachten.WpfStefWouters();
            wpf.Show();
        }
        
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            this.Close();
            login.Show();
        }
    }
}
