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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.VisualBasic;

namespace Examen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] medewerkers = new string[] { "Koen", "Sander", "Kristof" };
        private List<string> lstMedewerkers;
        private Dictionary<String, decimal> dictMenu = new Dictionary<String, decimal> { { "Dagmenu", 6.25m }, { "Pastamenu", 4.75m }, 
                                                          { "Veggie menu", 4.00m }, { "Soep", 2.15m } };
        private string[,] menuOverzicht;
        private string[] menus = new string[] { "Dagmenu", "Pastamenu", "Veggie menu", "Soep" };
        private decimal[] kortingTypePersoon = new decimal[] { 0.15m, 0.05m, 0.00m };
        private decimal kortingMindervalide = 0.30m;
        private int radioButtonSelected = 2;
        private string medewerkerSelected;
        private DateTime tijd;
        DispatcherTimer wekker;

        public MainWindow()
        {
            InitializeComponent();
            Windows_Loaded();

            KeyDown += new KeyEventHandler(KeyPressed);
            //OnClosing += new ExitEventHandler(Exit);
        }

        private void Windows_Loaded()
        {
            lstMedewerkers = new List<string>(medewerkers);
            foreach (string s in lstMedewerkers)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = s
                };
                ComboboxMedewerker.Items.Add(item);
            }

            menuOverzicht = InitialiseerArray();
            string output = "";
            int i = 0;
            foreach (string s in menuOverzicht)
            {
                output += s + "      ";
                if (i%2 == 1)
                {
                    output += "\n";
                }
                i++;
            }
            LabelMenuPrijzen.Content = output;

            wekker = new DispatcherTimer();
            wekker.Tick += new EventHandler(DispatcherTimer_Tick);
            wekker.Interval = new TimeSpan(0, 0, 1);
            tijd = DateTime.Now;
            LabelDateTime.Content = $"{tijd.ToLongDateString()} {tijd.ToLongTimeString()}";
            wekker.Start();

            ListBoxMenus.Items.Clear();
        }

        private string[,] InitialiseerArray()
        {
            menuOverzicht = new string[4, 2];
            int i = 0;
            foreach (KeyValuePair<String, decimal> kv in dictMenu)
            {
                menuOverzicht[i, 0] = kv.Key;
                menuOverzicht[i, 1] = kv.Value.ToString();
                i++;
            }
            return menuOverzicht;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            string name = radioButton.Content.ToString();
            switch (name)
            {
                case "Student":
                    radioButtonSelected = 0;
                    break;
                case "Lector":
                    radioButtonSelected = 1;
                    break;
                case "Gast":
                    radioButtonSelected = 2;
                    break;
            }
        }

        private void ButtonMenuToevoegen_Click(object sender, RoutedEventArgs e)
        {
            if (ComboboxMenus.SelectedItem != null)
            {
                ListBoxItem item = new ListBoxItem
                {
                    Content = menus[ComboboxMenus.SelectedIndex]
                };
                ListBoxMenus.Items.Add(item);
            }
        }

        private void ButtonItemVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxMenus.SelectedItem != null)
            {
                ListBoxMenus.Items.Remove(ListBoxMenus.SelectedItem);
            }
        }

        private void ButtonMedewerkerToevoegen_Click(object sender, RoutedEventArgs e)
        {
            string antwoord = Interaction.InputBox("Geef de naam van de medewerker?", "Medewerker toevoegen.");
            if (!string.IsNullOrEmpty(antwoord))
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = antwoord
                };
                ComboboxMedewerker.Items.Add(item);
            }
        }

        //Meerdere items van dezelfde soort worden op één lijn getoond ipv op meerdere lijnen
        private void ButtonBereken_Click(object sender, RoutedEventArgs e)
        {
            string output;
            decimal totaalprijs = 0.00m;
            decimal[] prijzen = new decimal[4];
            int[] menuVoorkomen = new int[4];
            if (ListBoxMenus.Items.Count != 0)
            {
                foreach (ListBoxItem item in ListBoxMenus.Items)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (item.Content.ToString() == menus[i])
                        {
                            decimal basisprijs = dictMenu[item.Content.ToString()];
                            int test = radioButtonSelected;
                            decimal korting = (bool)CheckBoxMindervalide.IsChecked ? basisprijs * kortingMindervalide
                                              : basisprijs * kortingTypePersoon[radioButtonSelected];
                            prijzen[i] += basisprijs - korting;

                            menuVoorkomen[i]++;
                        }
                    }
                }
                output = "PXL kassasysteem \n" +
                         $"Medewerker ID: {ComboboxMedewerker.SelectionBoxItem}\n";
                int j = 0;
                foreach (decimal prijs in prijzen)
                {
                    if (menuVoorkomen[j] != 0)
                    {
                        output += $"{menuVoorkomen[j]}x {menus[j]} ({dictMenu[menus[j]]}) = €{prijzen[j]} \n";
                    }
                    totaalprijs += prijs;
                    j++;
                }

                output += $"\nDe totale prijs: €{totaalprijs}";
                TextBoxOutput.Text = output;
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            LabelDateTime.Content = $"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}";
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            RadioButton1.IsChecked = false;
            RadioButton2.IsChecked = false;
            RadioButton3.IsChecked = false;
            ComboboxMenus.SelectedIndex = 0;
            CheckBoxMindervalide.IsChecked = false;
            ListBoxMenus.Items.Clear();
            TextBoxOutput.Text = "";
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    LabelKortingInfo.Content = $"Gast: {kortingTypePersoon[2]}% korting\n" +
                                               $"Lector: {kortingTypePersoon[1]}% korting\n" +
                                               $"Student: {kortingTypePersoon[0]}% korting\n";
                    LabelF2Info.Content = "Druk op F2 voor de info te verbergen.";
                    break;
                case Key.F2:
                    LabelKortingInfo.Content = "";
                    LabelF2Info.Content = "";
                    break;
            }
        }

        private void Afsluiten(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void OnClosing()
        {

        }
    }
}
