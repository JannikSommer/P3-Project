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
using Networking;
using System.Threading;
using Model.Log;
using Localization;
using Central_Controller;
using Model;
using Central_Controller.IO;

namespace WPF_PC
{
    public enum Language
    {
        Danish, English
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Item // remember to use item class from model
        {
            public string itemID { get; set; }
            public string itemName { get; set; }
            public string itemLocation { get; set; }
            public bool itemHasBeenCounted { get; set; }
            public int itemInStorageCount { get; set; }
            public int itemServerCount { get; set; }
            public int itemCountVariation { get; set; }
        }

        private Thread NetworkingThread { get; set; }
        private Controller Controller { get; set; }
        private Server Server { get; set; }
        private Thread NetworkingThread; // Used to keep socket connection open for clients. 
        private IOController _ioController = new IOController("TestCycle");

        private new Language Language { get; set; }

        public MainWindow()
        {
            Controller = new Controller();

            InitializeComponent();
            // StartServer();
            UpdateAllUI();
            LoadIntoDataGrid();
            LoadIntoChooseBox();

            // Add eksample data to save files.
            if(true) {
                List<LogMessage> list = new List<LogMessage> {
                new VerificationLogMessage(new DateTime(2019, 11, 12, 10, 21, 9), "Polle", "5709216007104", true),
                new LocationLogMessage(new DateTime(2019, 11, 12, 10, 21, 9), "Ole", "001C27", new List<(string itemId, string countedQuantity)>{("5709216007104", "5"), ("5849225908104", "2")}),
                new TextLogMessage(DateTime.Now, "Hello Bob!")
                };
                _ioController.CountedItems.AddRange(new List<string> { "12", "123456", "135612455" });
                _ioController.Log.AddMultipleMessages(list);
            }

        }

        private void StartServer()
        {
            Server = new Server(Controller);
            Thread NetworkingThread = new Thread(new ThreadStart(Server.StartServer));
            NetworkingThread.Start();
        }

        public void UpdateAllUI()
        {
            UpdateMainWindow();
            LoadIntoDataGrid();
            LoadIntoChooseBox();
        }

        public void LoadIntoDataGrid()
        {
            Item itemOne = new Item();

            itemOne.itemID = "12345";
            itemOne.itemName = "Hvid T-Shirt";
            itemOne.itemLocation = "001E02, 001F02";
            itemOne.itemHasBeenCounted = true;
            itemOne.itemInStorageCount = 34;
            itemOne.itemServerCount = 35;
            itemOne.itemCountVariation = 1;

            Item itemTwo = new Item();
            itemTwo.itemID = "12344";
            itemTwo.itemName = "Sort T-Shirt";
            itemTwo.itemLocation = "001E03, 001F03";
            itemTwo.itemHasBeenCounted = true;
            itemTwo.itemInStorageCount = 23;
            itemTwo.itemServerCount = 23;
            itemTwo.itemCountVariation = 0;
            dataGridMain.Items.Add(itemTwo);

            dataGridMain.Items.Add(itemOne);

        }

        public void UpdateMainWindow()
        {
            //Active Clients:
            acticeClients.Content = Controller.Active_Clients.Count;

            //Counted Items overview:
            double countedInt = 19843;
            double totalItemsInt = 80000;
            double persentageCounted = ((countedInt / totalItemsInt) * 100);
            double persentageCountedRoundedDown = Math.Round(persentageCounted, 1);

            overviewTotalCounted.Content = (countedInt + " / " + totalItemsInt + "   (" + persentageCountedRoundedDown + "%)");

            //Counted with difference overview:
            double countedIntWithDifference = 340;
            double persentageCountedWithDifference = ((countedIntWithDifference / totalItemsInt) * 100);
            double persentageCountedWithDifferenceRoundedDown = Math.Round(persentageCountedWithDifference, 1);

            overviewTotalCountedWithDifference.Content = (countedIntWithDifference + " / " + totalItemsInt + "   (" + persentageCountedWithDifferenceRoundedDown + "%)");
        }

        private void createCycleCount_Click(object sender, RoutedEventArgs e)
        {
            CreateCycleWindow CreateCycle = new CreateCycleWindow();
            CreateCycle.Show();

        }

        private void editCycle_Click(object sender, RoutedEventArgs e)
        {
            EditCycle EditCycle = new EditCycle();
            EditCycle.Show();

        }

        private void showLog_Click(object sender, RoutedEventArgs e)
        {
            

            LogWindow logWindow = new LogWindow(_ioController.Log);
            logWindow.Show();
        }

        private void finishCycle_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(((Language.Danish == Language) ? "ER DU SIKKER?" : "ARE YOU SURE?"), ((Language.Danish == Language) ? "Færdiggør cyklus'en?" : "Finish the cycle?"), MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
                
            }
            else
            {
                //do yes stuff

            }

        }

        public void LoadIntoChooseBox()
        {
            List<string> settings = new List<string> { 
                Localization.Resources.MainWindowComboboxCountedToday, 
                Localization.Resources.MainWindowComboboxCountedThisCycle, 
                Localization.Resources.MainWindowComboboxCountedDifference
            };
           
            comboBoxChooseGet.ItemsSource = settings;
            comboBoxChooseGet.SelectedIndex = 0;
        }

        private void showChosenType_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxChooseGet.SelectedIndex == -1)
            {
                labelWarning.Visibility = Visibility.Visible;
            }
            else
            {
                labelWarning.Visibility = Visibility.Hidden;

                //Todays Counted
                if (comboBoxChooseGet.SelectedIndex == 0)
                {

                }
                //Counted in this cycle
                else if(comboBoxChooseGet.SelectedIndex == 1)
                {

                }
                //Counted with difference
                else if (comboBoxChooseGet.SelectedIndex == 2)
                {

                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Load everything to log.
            Server.ShutdownServer();

            _ioController.Save();
        }

        private void comboBoxChooseGet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //e.AddedItems[0].ToString()
        }

        private void changeLanguage_Click(object sender, RoutedEventArgs e)
        {
            //System.Globalization.CultureInfo.CurrentUICulture = new System.Globalization.CultureInfo("da-DK", true);
            //InitializeComponent();
            ////if (Language == Language.Danish)
            ////{
            ////    Language = Language.English;
            ////}
            ////else
            ////{
            ////    Language = Language.Danish;
            ////}
            ////MainWindowLanguage();
            ////LoadIntoChooseBox();
        }  
        
        public void MainWindowLanguage()
        {

            //Danish:
            if (Language.Danish == Language)
            {
                //Buttons

                changeLanguage.Content = ("English");
                createCycleCount.Content = ("Opret Optællings Cyklus");
                editCycle.Content = ("Rediger Cyklus");
                showChosenType.Content = ("Vis");
                showLog.Content = ("Vis log");
                finishCycle.Content = ("Færdiggør Cyklus");

                //Label

                labelWarning.Content = ("Vælg venligst den ønskede funktion");
                activeClients.Content = ("Aktive klienter:");
                totalCount.Content = ("Total Optalt:");
                totalCountDifference.Content = ("Optalte med difference:");

                // Datagrid

                dataGridMain.Columns[0].Header = " ID";
                dataGridMain.Columns[1].Header = " Navn";
                dataGridMain.Columns[2].Header = " Lokationer";
                dataGridMain.Columns[3].Header = " Optalt";
                dataGridMain.Columns[4].Header = " Optalt Fra Lageret";
                dataGridMain.Columns[5].Header = " Antal Fra Serveren";
                dataGridMain.Columns[6].Header = " Difference";
            }

            //English:
            else if (Language.English == Language)
            {
                //Buttons

                changeLanguage.Content = ("Dansk");
                createCycleCount.Content = ("Create Cycle Count");
                editCycle.Content = ("Edit Cycle");
                showChosenType.Content = ("Show");
                showLog.Content = ("Show Log");
                finishCycle.Content = ("Finish Cycle");

                //Label

                labelWarning.Content = ("Please choose the desired function");
                activeClients.Content = ("Active clients:");
                totalCount.Content = ("Total Counted:");
                totalCountDifference.Content = ("Counted with difference:");

                // Datagrid

                dataGridMain.Columns[0].Header = " ID";
                dataGridMain.Columns[1].Header = " Name";
                dataGridMain.Columns[2].Header = " Locations";
                dataGridMain.Columns[3].Header = " Counted";
                dataGridMain.Columns[4].Header = " Counted from storage";
                dataGridMain.Columns[5].Header = " Count from server";
                dataGridMain.Columns[6].Header = " Difference";
            }
        }
    }
}
