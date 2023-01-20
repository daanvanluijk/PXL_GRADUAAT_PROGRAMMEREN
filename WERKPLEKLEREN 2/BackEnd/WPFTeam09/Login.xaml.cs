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

namespace WPFTeam09
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        DataSet dsLogin = new DataSet("Login");
        DataTable dtUser = new DataTable("Users");
        
        public Login()
        {
            InitializeComponent();

            dtUser.Columns.Add("UserId", typeof(int));
            dtUser.Columns.Add("User", typeof(string));
            dtUser.Columns.Add("Password", typeof(string));

            dsLogin.Tables.Add(dtUser);

            dtUser.Rows.Add(1, "Admin", "Admin123");
            dtUser.Rows.Add(2, "Stef", "123");
            dtUser.Rows.Add(3, "Daan", "123");
            dtUser.Rows.Add(4, "Kristof", "123");
            dtUser.Rows.Add(5, "Christos", "123");            
            
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            DataRow[] result = dtUser.Select($"User = '{txbUsername.Text}' and Password = '{pbPassword.Password}'");
            
            if (result.Length > 0)
            {
                AdminMenu adminMenu = new AdminMenu();
                this.Close();
                adminMenu.Show();
            }
            else
            {
                MessageBox.Show("Login is niet correct");
                return;
            }
            

        }
    }
}
