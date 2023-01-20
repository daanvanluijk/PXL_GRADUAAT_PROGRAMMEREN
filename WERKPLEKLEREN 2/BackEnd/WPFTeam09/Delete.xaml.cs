using ClassLibTeam09.Data.Framework;
using ClassLibTeam09.Entities;
using ClassLibTeam09.Settings;
using ClassLibTeam09.TableManagers;
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
using System.Windows.Threading;

namespace WPFTeam09
{
    /// <summary>
    /// Interaction logic for Delete.xaml
    /// </summary>
    public partial class Delete : Window
    {
        DataSet ds;
        DataSet ds2;
        DataSet ds3;
        string connectionString = Settings.Database.PxlConnectionString;
        int currentID;
        public Delete()
        {
            InitializeComponent();
            Time();
            GetDataUsers();
            //GetDataRooms();
        }

        //Get data in DataGrid
        private void GetDataUsers()
        {
            SelectResult selectResult = UsersManager.SelectUsers();
            ds = selectResult.DataTable.DataSet;
            dgMain.ItemsSource = ds.Tables[0].DefaultView;
            lblTableName.Content = "Gebruikers";
        }

        private void GetDataRooms()
        {
            SelectResult selectResult = RoomsManager.SelectRooms();
            ds2 = selectResult.DataTable.DataSet;
            dgMain.ItemsSource = ds2.Tables[0].DefaultView;
            lblTableName.Content = "Kamers";
        }
        
        private void GetDataOrders()
        {
            SelectResult selectResult = OrdersManager.SelectOrders();
            ds3 = selectResult.DataTable.DataSet;
            dgMain.ItemsSource = ds3.Tables[0].DefaultView;
            lblTableName.Content = "Orders";
        }

        //Search Data in DataGrid

        private void Search()
        {
            Dictionary<string, DataSet> lookup = new Dictionary<string, DataSet>()
            {
                ["Gebruikers"] = ds,
                ["Kamers"] = ds2,
                ["Orders"] = ds3,
            };
            SearchThroughTable(lookup[lblTableName.Content.ToString()]);
        }

        private void SearchThroughTable(DataSet ds)
        {
            string userinput = txbSearch.Text;
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    if (userinput.ToLower() == ds.Tables[0].Rows[j][i].ToString().ToLower())
                    {
                        DataGridRow row = (DataGridRow)dgMain.ItemContainerGenerator.ContainerFromIndex(j);
                        dgMain.SelectedIndex = j;
                        row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        return;
                    }
                }
            }
        }

        //Delete Data in DataGrid

        private void DeleteUser()
        {
            int id = dgMain.SelectedIndex;
            int userid = int.Parse(ds.Tables[0].Rows[id][0].ToString());
            UsersManager.DeleteUserWhereUserID(new User() { UserId = userid });
            GetDataUsers();
        }

        private void DeleteKamer()
        {
            int id = dgMain.SelectedIndex;
            int roomid = int.Parse(ds2.Tables[0].Rows[id][0].ToString());
            RoomsManager.DeleteRoomWhereRoomID(new Room() { RoomID = roomid });
            GetDataRooms();
        }

        private void DeleteOrder()
        {
            int id = dgMain.SelectedIndex;
            int orderid = int.Parse(ds3.Tables[0].Rows[id][0].ToString());
            OrdersManager.DeleteOrderWhereOrderID(new Order() { OrderID = orderid });
            GetDataOrders();
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ben je zeker dat je dit wilt verwijderen?", "Verwijder", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                if (lblTableName.Content == "Gebruikers" && dgMain.SelectedIndex > 0)
                {
                    DeleteUser();
                }
                else if (lblTableName.Content == "Kamers" && dgMain.SelectedIndex > 0)
                {
                    DeleteKamer();
                }
                else if (lblTableName.Content == "Orders" && dgMain.SelectedIndex > 0)
                {
                    DeleteOrder();
                }
               
            }
        }

        //Navigation buttons
        
        private void btnAfmelden_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            this.Close();
            login.Show();
        }

        private void btnKamerToevoegen_Click(object sender, RoutedEventArgs e)
        {
            AddRoom add = new AddRoom();
            this.Close();
            add.Show();
        }
        
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AdminMenu adminMenu = new AdminMenu();
            this.Close();
            adminMenu.Show();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }
               
        private void btnUserTable_Click(object sender, RoutedEventArgs e)
        {
            GetDataUsers();
        }

        private void btnRoomTable_Click(object sender, RoutedEventArgs e)
        {
            GetDataRooms();
        }
        
        private void btnOrdersTable_Click(object sender, RoutedEventArgs e)
        {
            GetDataOrders();
        }

        //Timer

        private void Time()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            lblTime.Content = $"{DateTime.Now.ToLongDateString()}   {DateTime.Now.ToLongTimeString()}";
        }
    }
}
