using ClassLibTeam09.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace WPFTeam09.Induviduele_opdrachten
{
    /// <summary>
    /// Interaction logic for WpfDaanVanLuik.xaml
    /// </summary>
    public partial class WpfDaanVanLuik : Window
    {
        public WpfDaanVanLuik()
        {
            InitializeComponent();
        }


        private void btnSelecttestSql_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(Settings.Database.IndividualConnectionstring);
                string query = "select * from student";
                SqlCommand sql = new SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = sql;
                var ds = new DataSet();
                adapter.Fill(ds);
                conn.Close();
                dgStudents.ItemsSource = ds.Tables[0].DefaultView;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }




        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(Settings.Database.IndividualConnectionstring);
                string query = $"insert into student  (voornaam, achternaam) VALUES('{txbVoornaam.Text}','{txbAchternaam.Text}' ) ";
                SqlCommand sql = new SqlCommand(query, conn);
                conn.Open();
                sql.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
