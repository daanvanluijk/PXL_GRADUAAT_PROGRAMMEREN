using ClassLibTeam09.Mail;
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
    /// Interaction logic for WpfStefWouters.xaml
    /// </summary>
    public partial class WpfStefWouters : Window
    {
        public WpfStefWouters()
        {
            InitializeComponent();
        }

        private void btnSelecttestSql_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(Settings.Database.ConnectionString);
                //string query = "select * from students";
                string query = $"select SUM(sq.PRICE) TOTAL_PRICE from (select SUM(f.facilityPrice) PRICE from Facility_Orders fo " +
                    $"join Facilities f on f.facilityID = fo.facilityID where fo.orderID = 1 group by fo.orderID union select SUM(r.roomPrice) " +
                    $"from Room_Orders ro join Rooms r on r.roomID = ro.roomID where ro.orderID = 1 group by ro.orderID) sq; ";
                SqlCommand sql = new SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = sql;
                var ds = new DataSet();
                adapter.Fill(ds);                
                conn.Close();
                dgStudents.ItemsSource = ds.Tables[0].DefaultView;
             
                //TextBlock test = dgStudents.Columns[0].GetCellContent(dgStudents.Items[0]) as TextBlock;
                MessageBox.Show(ds.Tables[0].Rows[0][0].ToString());

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
                //string query = $"delete from Students where studentId = {txbAchternaam.Text};"; //Delete query
                //string query = $"UPDATE Students SET FirstName = '{txbVoornaam.Text}', LastName = '{txbAchternaam.Text}' WHERE studentId = 3;"; //Update query
                string query = $"insert into students (Firstname, Lastname) VALUES('{txbVoornaam.Text}','{txbAchternaam.Text}' ) ";  //Insert query                
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

        private void btnTestMail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mail = new BoekersMail("christos.lappas@outlook.com");
                mail.Body = "test";
                mail.Subject = "test";
                mail.SendMail();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
