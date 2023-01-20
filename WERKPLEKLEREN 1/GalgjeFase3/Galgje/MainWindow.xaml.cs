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
using System.Windows.Threading;

namespace Galgje
{
    public partial class MainWindow : Window
    {
        private enum State
        {
            GeheimWoordIngeven,
            Raden,
            Gewonnen,
            Verloren,
            Menu,
            Scoreboard,
        }
        private State state;
        private State previousState;
        private enum PlayerMode
        {
            Singleplayer,
            Multiplayer,
        }
        private PlayerMode playerMode = PlayerMode.Singleplayer;

        private const int maxLevens = 10;
        private const int minimumLetters = 3;

        private int levens;
        private string juisteLetters;
        private string fouteLetters;
        private string woord;
        private string woordMask;

        private DispatcherTimer resetTextTimer = new DispatcherTimer();
        private DispatcherTimer beurtTimer = new DispatcherTimer();
        private int beurtTimerSeconds = 10;
        private int beurtTimerSecondsLeft = 10;
        private bool beurtTimerPaused = false;

        private string[] woordenlijst;
        private string[] wordlist;
        private string[] singlePlayerWoordenlijst;

        private Array[] scoreboard = new Array[10];
        private bool usedHint = false;

        public MainWindow()
        {
            InitializeComponent();

            gridMain.Visibility = Visibility.Hidden;
            gridScoreboard.Visibility = Visibility.Hidden;

            buttonContinue.IsEnabled = false;
            buttonGeefHint.IsEnabled = false;

            State_Change(State.Menu);

            resetTextTimer.Tick += ResetTextTimer_Tick;
            beurtTimer.Tick += BeurtTimer_Tick;
            beurtTimer.Interval = TimeSpan.FromMilliseconds(1000);

            GetDictionary(ref woordenlijst, @"woordenlijst.txt");
            GetDictionary(ref wordlist, @"wordlist.txt");
            GetDictionary(ref singlePlayerWoordenlijst, @"singlePlayerWoordenlijst.txt");

            KeyDown += new KeyEventHandler(KeyPressed);
            SizeChanged += new SizeChangedEventHandler(WindowResized);
        }

        private void WindowResized(object sender, SizeChangedEventArgs e)
        {
            int fontSize;
            if (e.NewSize.Height < e.NewSize.Width)
            {
                fontSize = (int)Math.Round(e.NewSize.Height / 25);
            }
            else
            {
                fontSize = (int)Math.Round(e.NewSize.Width / 25);
            }
            buttonNieuwSpel.FontSize = fontSize;
            buttonRaad.FontSize = fontSize;
            buttonVerbergWoord.FontSize = fontSize;
            labelOutput.FontSize = fontSize;
            labelTimer.FontSize = fontSize;
            textBoxInput.FontSize = fontSize;
            labelAantalSpelers.FontSize = fontSize;
            labelTimerLengte.FontSize = fontSize;
            labelAantalSpelersLabel.FontSize = fontSize;
            labelTimerLengteLabel.FontSize = fontSize;
            buttonAantalSpelersMin.FontSize = fontSize;
            buttonAantalSpelersPlus.FontSize = fontSize;
            buttonTimerLengteMin.FontSize = fontSize;
            buttonTimerLengtePlus.FontSize = fontSize;
            buttonGeefHint.FontSize = fontSize;
            buttonScoreboard.FontSize = fontSize;
            buttonPauze.FontSize = fontSize;
            buttonAfsluiten.FontSize = fontSize;
            buttonContinue.FontSize = fontSize;
            labelScoreboard.FontSize = fontSize;
            buttonMenuFromScoreboard.FontSize = fontSize;
            gridScoreboardSettings.MaxWidth = e.NewSize.Width;
        }

        #region buttonCallbacks
        private void ButtonVerbergWoord_Click(object sender, RoutedEventArgs e)
        {
            if (state == State.GeheimWoordIngeven)
            {
                if (playerMode == PlayerMode.Multiplayer)
                {
                    if (CheckStringForLettersOnly(RemoveSpaces(textBoxInput.Text)))
                    {
                        if (RemoveSpaces(textBoxInput.Text).Length > minimumLetters)
                        {
                            if (CheckForWordInLibrary(RemoveSpaces(textBoxInput.Text)))
                            {
                                woord = RemoveSpaces(textBoxInput.Text).ToLower();
                                woordMask = GenerateWordMask();
                                State_Change(State.Raden);
                            }
                            else
                            {
                                SetLabelForXMilliSeconds("Woord staat niet \nin het woordenboek", 1500);
                            }
                        }
                        else
                        {
                            SetLabelForXMilliSeconds("Woord moet meer dan \n3 letters bevatten", 1500);
                        }
                    }
                    else
                    {
                        SetLabelForXMilliSeconds("Woord mag alleen \nletters bevatten", 1500);
                    }
                }
                else if (playerMode == PlayerMode.Singleplayer)
                {
                    Random random = new Random();
                    woord = singlePlayerWoordenlijst[random.Next(0, singlePlayerWoordenlijst.Count())];
                    woordMask = GenerateWordMask();
                    State_Change(State.Raden);
                }
            }
        }

        private void ButtonNieuwSpel_Click(object sender, RoutedEventArgs e)
        {
            playerMode = (PlayerMode)int.Parse(labelAantalSpelers.Content.ToString()) - 1;
            beurtTimerSeconds = int.Parse(labelTimerLengte.Content.ToString());
            labelTimer.Content = beurtTimerSeconds;
            State_Change(State.GeheimWoordIngeven);
        }

        private void ButtonRaad_Click(object sender, RoutedEventArgs e)
        {
            if (state == State.Raden)
            {
                if (CheckStringForLettersOnly(RemoveSpaces(textBoxInput.Text)))
                {
                    if (char.TryParse(RemoveSpaces(textBoxInput.Text), out char c))
                    {
                        if (CheckForLetterInWord(c))
                        {
                            AddToJuisteLetters(c);
                            if (CheckIfInputsMatch(woord, juisteLetters))
                            {
                                State_Change(State.Gewonnen);
                            }
                            else
                            {
                                woordMask = GenerateWordMask();
                                State_Refresh();
                            }
                        }
                        else
                        {
                            DecreaseLevens();
                            AddToFouteLetters(c);
                        }
                    }
                    else
                    {
                        if (CheckIfInputsMatch(RemoveSpaces(textBoxInput.Text), woord))
                        {
                            State_Change(State.Gewonnen);
                        }
                        else
                        {
                            DecreaseLevens();
                            SetLabelForXMilliSeconds("Je hebt het woord niet geraden", 1500);
                        }
                    }
                }
                else
                {
                    SetLabelForXMilliSeconds("Woord mag alleen \nletters bevatten", 1500);
                }
            }
            else if (state == State.Gewonnen)
            {
                if (textBoxInput.Text != "")
                {
                    labelScoreboard.Content = "";
                    object[] scoreEntry = new object[3] { RemoveSpaces(textBoxInput.Text), levens, "" };
                    for (int i = 0; i < scoreboard.Length; i++)
                    {
                        if (scoreboard[i] is object[] o)
                        {
                            if (int.Parse(scoreEntry[1].ToString()) > int.Parse(o[1].ToString()))
                            {
                                for (int j = scoreboard.Length - 1; j > i; j--)
                                {
                                    scoreboard[j] = scoreboard[j - 1];
                                }
                                scoreboard[i] = scoreEntry;
                                break;
                            }
                        }
                        else
                        {
                            scoreboard[i] = scoreEntry;
                            break;
                        }
                    }
                    foreach (object i in scoreboard)
                    {
                        if (i is object[] o)
                        {
                            labelScoreboard.Content += $"{o[0]} - {o[1]} Levens over - {DateTime.Now:HH:mm:ss}\n";
                        }
                    }
                    State_Change(State.Scoreboard);
                    buttonContinue.IsEnabled = false;
                }
                else
                {
                    SetLabelForXMilliSeconds("Geef een naam in", 1500);
                }
            }
        }

        private void ButtonPauze_Click(object sender, RoutedEventArgs e)
        {
            State_Change(State.Menu);
        }

        private void ButtonContinue_Click(object sender, RoutedEventArgs e)
        {
            State_Change(previousState);
        }

        private void ButtonGeefHint_Click(object sender, RoutedEventArgs e)
        {
            State_Change(previousState);
            Random random = new Random();
            string mogelijkeLetters = "abcdefghijklmnopqrstvwxyz";
            foreach (char c in woord)
            {
                mogelijkeLetters = mogelijkeLetters.Replace(c.ToString(), "");
            }
            foreach (char c in fouteLetters)
            {
                mogelijkeLetters = mogelijkeLetters.Replace(c.ToString(), "");
            }
            if (mogelijkeLetters != "")
            {
                char[] mogelijkeKarakters = mogelijkeLetters.ToCharArray();
                char hintLetter = mogelijkeKarakters[random.Next(0, mogelijkeKarakters.Length)];
                AddToFouteLetters(hintLetter);
                SetLabelForXMilliSeconds($"De letter '{hintLetter}' zit niet in het woord", 3000);
                usedHint = true;
            }
            else
            {
                SetLabelForXMilliSeconds($"Geen hints meer mogelijk", 3000);
            }
        }

        private void ButtonScoreboard_Click(object sender, RoutedEventArgs e)
        {
            State_Change(State.Scoreboard);
        }

        private void Quit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SettingsKnop_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name)
            {
                default:
                case "buttonAantalSpelersMin":
                    int newValue = int.Parse(labelAantalSpelers.Content.ToString()) - 1;
                    if (newValue >= 1)
                    {
                        labelAantalSpelers.Content = newValue;
                    }
                    break;
                case "buttonAantalSpelersPlus":
                    newValue = int.Parse(labelAantalSpelers.Content.ToString()) + 1;
                    if (newValue <= 2)
                    {
                        labelAantalSpelers.Content = newValue;
                    }
                    break;
                case "buttonTimerLengteMin":
                    newValue = int.Parse(labelTimerLengte.Content.ToString()) - 1;
                    if (newValue >= 5)
                    {
                        labelTimerLengte.Content = newValue;
                    }
                    break;
                case "buttonTimerLengtePlus":
                    newValue = int.Parse(labelTimerLengte.Content.ToString()) + 1;
                    if (newValue <= 20)
                    {
                        labelTimerLengte.Content = newValue;
                    }
                    break;
            }
        }
        #endregion

        #region stateMethods
        //Deze functie past de huidige state aan en herlaad de state
        private void State_Change(State s)
        {
            if (s != State.Scoreboard)
            {
                previousState = state;
            }
            state = s;
            State_Refresh();
        }

        //Deze functie herlaad de huidige staat
        private void State_Refresh()
        {
            switch (state)
            {
                default:
                case State.Menu:
                    StateCaseMenu();
                    break;
                case State.GeheimWoordIngeven:
                    StateCaseGeheimWoordIngeven();
                    break;
                case State.Raden:
                    StateCaseRaden();
                    break;
                case State.Gewonnen:
                    StateCaseGewonnen();
                    break;
                case State.Verloren:
                    StateCaseVerloren();
                    break;
                case State.Scoreboard:
                    StateCaseScoreboard();
                    break;
            }
        }

        //De volgende functies bevatten de individuele setup parameters van elke staat
        private void StateCaseMenu()
        {
            gridPauseScreen.Visibility = Visibility.Visible;
            gridMain.Visibility = Visibility.Hidden;
            gridScoreboard.Visibility = Visibility.Hidden;
            resetTextTimer.Stop();
            beurtTimer.Stop();
            beurtTimerPaused = true;
        }

        private void StateCaseGeheimWoordIngeven()
        {
            buttonVerbergWoord.Visibility = Visibility.Visible;
            if (playerMode == PlayerMode.Singleplayer)
            {
                buttonVerbergWoord.Content = "Start";
                textBoxInput.Visibility = Visibility.Hidden;
                textBoxInput.Focusable = false;
                labelOutput.Content = "Klik op start om te beginnen";
            }
            else
            {
                buttonVerbergWoord.Content = "Verberg Woord";
                textBoxInput.Visibility = Visibility.Visible;
                textBoxInput.Focusable = true;    //Bug die ik niet krijg opgelost
                textBoxInput.Focus();             //De textbox krijgt geen focus in deze state
                _ = Keyboard.Focus(textBoxInput); //Maar in andere wel
                labelOutput.Content = "Speler 2, Geef een geheim woord in";
            }
            buttonRaad.Visibility = Visibility.Hidden;
            textBoxInput.Text = "";
            labelTimer.Visibility = Visibility.Hidden;
            levens = maxLevens;
            juisteLetters = "";
            fouteLetters = "";
            imageGalg.Source = new BitmapImage(new Uri(@"/fase0.png", UriKind.Relative));
            gridPauseScreen.Visibility = Visibility.Hidden;
            gridMain.Visibility = Visibility.Visible;
            buttonContinue.IsEnabled = true;
            buttonGeefHint.IsEnabled = false;
            usedHint = false;
        }

        private void StateCaseRaden()
        {
            buttonVerbergWoord.Visibility = Visibility.Hidden;
            buttonRaad.Visibility = Visibility.Visible;
            buttonRaad.IsEnabled = true;
            buttonRaad.Content = "Raad";
            labelTimer.Visibility = Visibility.Visible;
            textBoxInput.Visibility = Visibility.Visible;
            textBoxInput.Text = "";
            textBoxInput.Focusable = true;
            _ = Keyboard.Focus(textBoxInput);
            gridPauseScreen.Visibility = Visibility.Hidden;
            gridMain.Visibility = Visibility.Visible;
            buttonContinue.IsEnabled = true;
            buttonGeefHint.IsEnabled = true;
            if (!beurtTimerPaused)
            {
                StartBeurtTimer(beurtTimerSeconds + 1);
            }
            else
            {
                StartBeurtTimer(beurtTimerSecondsLeft);
                beurtTimerPaused = false;
            }
            Change_Label($"{levens} Levens \n" +
                                  $"Juiste letters: {juisteLetters} \n" +
                                  $"Foute letters: {fouteLetters} \n" +
                                  $"{woordMask}");
        }

        private void StateCaseGewonnen()
        {
            buttonRaad.Visibility = Visibility.Visible;
            buttonRaad.Content = "Voeg High Score Toe";
            if (usedHint)
            {
                buttonRaad.IsEnabled = false;
            }
            else
            {
                buttonRaad.IsEnabled = true;
            }
            labelTimer.Visibility = Visibility.Hidden;
            textBoxInput.Visibility = Visibility.Visible;
            textBoxInput.Text = "";
            beurtTimer.Stop();
            gridPauseScreen.Visibility = Visibility.Hidden;
            gridMain.Visibility = Visibility.Visible;
            buttonContinue.IsEnabled = true;
            buttonGeefHint.IsEnabled = false;
            Change_Label($"Je hebt \n" +
                                  $"\"{woord}\" \n" +
                                  $"correct geraden");
        }

        private void StateCaseVerloren()
        {
            buttonRaad.Visibility = Visibility.Hidden;
            labelTimer.Visibility = Visibility.Hidden;
            textBoxInput.Visibility = Visibility.Hidden;
            beurtTimer.Stop();
            gridPauseScreen.Visibility = Visibility.Hidden;
            gridMain.Visibility = Visibility.Visible;
            buttonContinue.IsEnabled = false;
            buttonGeefHint.IsEnabled = false;
            Change_Label($"Je hebt " +
                                  $"\"{woord}\" \n" +
                                  $"niet op tijd geraden");
        }

        private void StateCaseScoreboard()
        {
            gridMain.Visibility = Visibility.Hidden;
            gridPauseScreen.Visibility = Visibility.Hidden;
            gridScoreboard.Visibility = Visibility.Visible;
        }
        #endregion

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                switch (state)
                {
                    default:
                    case State.GeheimWoordIngeven:
                        if (buttonVerbergWoord.IsEnabled)
                            ButtonVerbergWoord_Click(null, null);
                        break;
                    case State.Raden:
                        if (buttonRaad.IsEnabled)
                            ButtonRaad_Click(null, null);
                        break;
                    case State.Gewonnen:
                        if (buttonRaad.IsEnabled)
                            ButtonRaad_Click(null, null);
                        break;
                    case State.Verloren:
                        if (buttonPauze.IsEnabled)
                            ButtonPauze_Click(null, null);
                        break;
                    case State.Menu:
                        if (buttonContinue.IsEnabled)
                            ButtonContinue_Click(null, null);
                        else
                            ButtonNieuwSpel_Click(null, null);
                        break;
                    case State.Scoreboard:
                        if (buttonPauze.IsEnabled)
                            ButtonPauze_Click(null, null);
                        break;
                }
            }
            if (e.Key == Key.Escape)
            {
                if (state == State.Menu)
                {
                    State_Change(previousState);
                }
                else
                {
                    State_Change(State.Menu);
                }
            }
        }

        //Deze functie verhindert dat de content reset wordt door de timer
        //wanneer de content al ergens anders door verandert wordt
        private void Change_Label(string s)
        {
            if ((string)labelOutput.Content != s)
            {
                labelOutput.Content = s;
                resetTextTimer.Stop();
            }
        }

        //Toont een message in het label en verhindert speler input
        private void SetLabelForXMilliSeconds(string s, int ms)
        {
            labelOutput.Content = s;
            resetTextTimer.Interval = TimeSpan.FromMilliseconds(ms);
            resetTextTimer.Stop();
            resetTextTimer.Start();
            buttonRaad.IsEnabled = false;
            beurtTimer.Stop();
            beurtTimerPaused = true;
        }

        #region timerMethods
        //Herlaad de huidige state
        private void StartBeurtTimer(int s)
        {
            beurtTimerSecondsLeft = s;
            beurtTimer.Stop();
            beurtTimer.Start();
        }

        private void ResetTextTimer_Tick(object sender, EventArgs e)
        {
            State_Refresh();
            resetTextTimer.Stop();
        }

        //Requirement 'GALG-10.2' was onduidelijk en werd geïnterpreteerd als: ALTIJD 1 seconde wachten voor de timer aftelt
        private void BeurtTimer_Tick(object sender, EventArgs e)
        {
            if (beurtTimerSecondsLeft > 0)
            {
                beurtTimerSecondsLeft -= 1;
                labelTimer.Background = Brushes.Transparent;
                labelTimer.Content = beurtTimerSecondsLeft.ToString();
            }
            else
            {
                DecreaseLevens();
                beurtTimer.Stop();
                labelTimer.Background = Brushes.Red;
                State_Refresh();
            }
        }
        #endregion

        #region stringCheckMethods
        private bool CheckStringForLettersOnly(string s)
        {
            return s.All(char.IsLetter);
        }

        private bool CheckForLetterInWord(char c)
        {
            return woord.Contains(c);
        }

        private bool CheckIfInputsMatch(string s1, string s2)
        {
            return s1 != "" && s1.All(e => s2.Contains(e)) && s2.All(e => s1.Contains(e));
        }

        private bool CheckForWordInLibrary(string s)
        {
            return woordenlijst.Contains(s) || wordlist.Contains(s);
        }
        #endregion

        #region stringManipulationMethods
        private string RemoveSpaces(string s)
        {
            s = s.Replace(" ", "");
            return s;
        }

        private string GenerateWordMask()
        {
            string word = "";
            foreach (char c in woord)
            {
                if (juisteLetters.Contains(c))
                {
                    word += c;
                }
                else
                {
                    word += "＿";
                }
                word += " ";
            }
            return word;
        }
        #endregion

        private void AddToFouteLetters(char c)
        {
            if (!fouteLetters.Contains(c))
            {
                fouteLetters += c;
                State_Refresh();
            }
            else
            {
                SetLabelForXMilliSeconds("Hey kwibus, dit heb je al \nfout geraden", 1500);
            }
        }

        private void AddToJuisteLetters(char c)
        {
            if (!juisteLetters.Contains(c))
            {
                juisteLetters += c;
                State_Refresh();
            }
            else
            {
                SetLabelForXMilliSeconds("Hey kwibus, dit heb je al \njuist geraden", 1500);
            }
        }

        private void DecreaseLevens()
        {
            levens -= 1;
            imageGalg.Source = new BitmapImage(new Uri(@"/fase" + -(levens - maxLevens) + ".png", UriKind.Relative));
            if (levens <= 0)
            {
                State_Change(State.Verloren);
            }
        }

        //Deze method instantiëert de verschillende woordenlijsten
        private void GetDictionary(ref string[] wordList, string filePath)
        {
            int c = File.ReadLines(filePath).Count();
            wordList = new string[c];
            int i = 0;
            foreach (string line in File.ReadLines(filePath))
            {
                wordList[i] = line;
                i++;
            }
        }
    }
}
