using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Threading;
using ClassLibTeam09.Settings;
using System.Windows.Controls;
using Image = System.Drawing.Image;
using ClassLibTeam09.TableManagers;
using ClassLibTeam09.Data.Framework;
using ClassLibTeam09.Entities;

namespace WPFTeam09
{
    /// "Dit bestand is één grote spaghetti en ik ga het niet oplossen, werk 'MET' de spaghetti niet 'TEGEN' de spaghetti" - Daan
    public partial class AddRoom : Window
    {
        DataSet dsImg;
        DataSet dsRoom;
        string imgLoc = "";
        int currentRoomID;
        DataView roomView = new DataView();
        List<Control> bedtypeControls = new List<Control>();

        public AddRoom()
        {
            InitializeComponent();
            SetupBedtypeControls();
            GetCurrentTime();
            ReloadRooms();
            //GetImg();
        }

        private void SetupBedtypeControls()
        {
            bedtypeControls.Add(combo1P);
            bedtypeControls.Add(combo2P);
            bedtypeControls.Add(comboKing);
            bedtypeControls.Add(comboZetel);
            bedtypeControls.Add(chkWifi);
            bedtypeControls.Add(chkAirco);
            bedtypeControls.Add(chkDouche);
            bedtypeControls.Add(chkBad);
        }

        private void ReloadImages()
        {
            SelectResult result = RoomImagesManager.SelectRoomImageWhereRoomID(new RoomImage(){RoomId = currentRoomID});
            dsImg = result.DataTable.DataSet;
            dataGrid2.ItemsSource = dsImg.Tables[0].DefaultView;
        }

        private void ReloadRooms()
        {
            SelectResult result = RoomsManager.SelectRooms();
            dsRoom = result.DataTable.DataSet;
            roomView = dsRoom.Tables[0].DefaultView;
            dataGrid.ItemsSource = roomView;
        }

        private ImageSource GetImageFromImgID(int imgid)
        {
            ClassLibTeam09.Entities.Image imageObject = new ClassLibTeam09.Entities.Image()
            {
                ImgId = imgid
            };
            SelectResult selectResult = ImagesManager.SelectImageWhereImgID(imageObject);
            imageObject.ImageUrl = ImagesManager.ConvertDataRowToImage(selectResult.DataTable).ImageUrl;

            using(var ms = new MemoryStream(imageObject.ImageUrl))
            {
                BitmapImage image = new BitmapImage();
                ms.Position = 0;
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }

        private void GetCurrentTime()
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

        private void btnKamerVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            Delete delete = new Delete();
            Close();
            delete.Show();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AdminMenu adminMenu = new AdminMenu();
            Close();
            adminMenu.Show();
        }

        private void btnAfmelden_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            Close();
            login.Show();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            int id = dataGrid.SelectedIndex;
            if (dsRoom.Tables[0].Rows[id][0].ToString() == "") return;
            int roomid = int.Parse(dsRoom.Tables[0].Rows[id][0].ToString());
            currentRoomID = roomid;
            ReloadImages();
            dataGrid2.ItemsSource = dsImg.Tables[0].DefaultView;
            dataGrid.Visibility = Visibility.Collapsed;
            dataGrid2.Visibility = Visibility.Visible;
        }

        private void btnBack_Click_1(object sender, RoutedEventArgs e)
        {
            dataGrid2.Visibility = Visibility.Collapsed;
            dataGrid.Visibility = Visibility.Visible;
        }

        private void btnFromDB_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.Visibility != Visibility.Visible) return;
            DataRowCollection rows = dsImg.Tables[0].Rows;
            DataRow row = rows[dataGrid2.SelectedIndex];
            image.Source = GetImageFromImgID(int.Parse(row[0].ToString()));
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            chkAirco.IsChecked = false;
            chkBad.IsChecked = false;
            chkDouche.IsChecked = false;
            chkWifi.IsChecked = false;
            combo1P.Text = "";
            combo2P.Text = "";
            comboKing.Text = "";
            comboZetel.Text = "";
            dataGrid.Visibility = Visibility.Visible;
            dataGrid2.Visibility = Visibility.Collapsed;
            ReloadRooms();
        }

        private void btnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            DataRowCollection rows = dsRoom.Tables[0].Rows;
            if (dataGrid.Visibility == Visibility.Visible && dataGrid.SelectedIndex < rows.Count && rows[dataGrid.SelectedIndex][0].ToString() != "")
            {
                currentRoomID = int.Parse(rows[dataGrid.SelectedIndex][0].ToString());
                DataRow row = rows[dataGrid.SelectedIndex];
                InsertRoomBedtype(row);
            }

            foreach (DataRow row in rows)
            {
                if (row[0].ToString() != "")
                {
                    UpdateRoom(row);
                }
                else if (row[0].ToString() == "" && row[1].ToString() != "" && row[2].ToString() != "" && row[3].ToString() != "" && row[4].ToString() != "")
                {
                    InsertRoom(row);
                }
            }

            MessageBox.Show("Kamer(s) Opgeslagen");

            btnReset_Click(null, null);
        }

        private void InsertRoomBedtype(DataRow row)
        {
            RoomBedtypesManager.DeleteRoomBedtypeWhereRoomID(new RoomBedtype() { RoomID = int.Parse(row[0].ToString()) });

            for (int i = 0; i < bedtypeControls.Count; i++)
            {
                int amount = 0;
                Control bc = bedtypeControls[i];
                if (bc is ComboBox combo)
                    amount = combo.SelectedIndex;
                else if (bc is CheckBox check)
                    amount = (bool)check.IsChecked ? 1 : 0;

                for (int j = 0; j < amount; j++)
                {
                    RoomBedtypesManager.InsertRoomBedtype(new RoomBedtype() { RoomID = int.Parse(row[0].ToString()), BedtypeId = i + 1 });
                }
            }
        }

        private void UpdateRoom(DataRow row)
        {
            Room room = new Room()
            {
                RoomID = int.Parse(row[0].ToString()),
                RoomPrice = int.Parse(row[1].ToString()),
                RoomCapacity = int.Parse(row[2].ToString()),
                Title = row[3].ToString(),
                Description = row[4].ToString(),
            };
            RoomsManager.UpdateRoomWhereRoomID(room);
        }

        private void InsertRoom(DataRow row)
        {
            Room room = new Room()
            {
                RoomPrice = int.Parse(row[1].ToString()),
                RoomCapacity = int.Parse(row[2].ToString()),
                Title = row[3].ToString(),
                Description = row[4].ToString(),
            };
            RoomsManager.InsertRoom(room);
        }

        private void btnVerwijderFoto_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.Visibility != Visibility.Visible) return;

            DataRowCollection rows = dsImg.Tables[0].Rows;
            if (dataGrid2.SelectedIndex >= rows.Count) return;

            DataRow row = rows[dataGrid2.SelectedIndex];
            RoomImagesManager.DeleteRoomImageWhereImgID(new RoomImage() { ImgId = int.Parse(row[0].ToString()) });

            MessageBox.Show("Afbeelding verwijderd");

            ReloadImages();
        }

        private void btnKies_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png";
            fd.Title = "Upload foto";
            if (fd.ShowDialog() == false)
                return;

            image.Source = new BitmapImage(new Uri(fd.FileName));
            imgLoc = fd.FileName.ToString();
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            DataRowCollection rows = dsRoom.Tables[0].Rows;
            if (dataGrid.Visibility == Visibility.Visible)
            {
                if (dataGrid.SelectedIndex == -1 || dataGrid.SelectedIndex >= rows.Count || rows[dataGrid.SelectedIndex][0].ToString() == "")
                {
                    MessageBox.Show("Kies een correcte rij!");
                    return;
                }
                currentRoomID = int.Parse(dsRoom.Tables[0].Rows[dataGrid.SelectedIndex][0].ToString());
            }
            if (currentRoomID == 0) return;

            if (imgLoc != string.Empty && currentRoomID != -1)
            {
                FileStream fs = new FileStream(imgLoc, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                byte[] img = br.ReadBytes((int)fs.Length);
                ClassLibTeam09.Entities.Image imageObject = new ClassLibTeam09.Entities.Image()
                {
                    ImageUrl = img
                };
                InsertResult insertResult = ImagesManager.InsertImage(imageObject);

                SelectResult result = ImagesManager.SelectLastImage();
                imageObject.ImgId = ImagesManager.ConvertDataRowToImage(result.DataTable).ImgId;

                RoomImage roomImage = new RoomImage()
                {
                    ImgId = imageObject.ImgId,
                    ImageUrl = imageObject.ImageUrl,
                    RoomId = currentRoomID
                };

                InsertResult insertResult2 = RoomImagesManager.InsertRoomImage(roomImage);
            }

            MessageBox.Show("Afbeelding(en) Opgeslagen");

            if (dataGrid.Visibility == Visibility.Visible) return;
            ReloadImages();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowCollection rows = dsRoom.Tables[0].Rows;
            if (dataGrid.Visibility != Visibility.Visible || dataGrid.SelectedIndex == -1 ||
                dataGrid.SelectedIndex >= rows.Count || rows[dataGrid.SelectedIndex][0].ToString() == "")
                return;
            int id = int.Parse(rows[dataGrid.SelectedIndex][0].ToString());
            SelectResult result = RoomBedtypesManager.SelectBedtypeIDWhereRoomID(new RoomBedtype() { RoomID = id });
            DataSet faciliteiten = result.DataTable.DataSet;

            int[] amounts = new int[4];
            chkWifi.IsChecked = false;
            chkAirco.IsChecked = false;
            chkDouche.IsChecked = false;
            chkBad.IsChecked = false;

            foreach (DataRow row in faciliteiten.Tables[0].Rows)
            {
                int value = int.Parse(row[0].ToString());
                if (bedtypeControls[value - 1] is ComboBox)
                {
                    amounts[value - 1]++;
                }
                else if (bedtypeControls[value - 1] is CheckBox box)
                {
                    box.IsChecked = true;
                }
            }
            for(int i = 0; i < 4; i++)
            {
                ComboBox box = (ComboBox)bedtypeControls[i];
                box.SelectedIndex = amounts[i];
            }
        }
    }
}
