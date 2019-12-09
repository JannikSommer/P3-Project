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
using System.Globalization;
using PrestaSharpAPI;


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

        private Thread NetworkingThread { get; set; }
        private Controller Controller { get; set; } = new Controller();
        public EditCycle EditCycleWindow;
        private Server Server { get; set; }
        //private Thread NetworkingThread; // Used to keep socket connection open for clients. 
        private IOController _ioController = new IOController("TestCycle");


        public MainWindow()
        {
            EditCycleWindow = new EditCycle(Controller);
            Controller.InitilizeLocationComparer(EditCycleWindow.RetrieveSortingPriorityFromFile());

            InitializeComponent();
            // StartServer();
            UpdateAllUI();
            LoadIntoDataGrid();
            LoadIntoChooseBox();

            // Add eksample data to save files.
            if(false) {
                List<LogMessage> list = new List<LogMessage> {
                new VerificationLogMessage(new DateTime(2019, 11, 12, 10, 21, 9), "Polle", "5709216007104", true),
                new LocationLogMessage(new DateTime(2019, 11, 12, 10, 21, 9), "Ole", "001C27", new List<(string itemId, string countedQuantity)>{("5709216007104", "5"), ("5849225908104", "2")}),
                new TextLogMessage(DateTime.Now, "Hello Bob!")
                };
                _ioController.CountedItems.AddRange(new List<Item> { new Item("135424", "Ugly T-Shirt", "Purple", "XL", new List <Location> { new Location("002F01") }), new Item("19753", "Nice T-Shirt", "Blue", "M", new List<Location> { new Location("002F01"), new Location("022D07") }) });
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
            if(false) {
                ProductAPI psAPI = new ProductAPI();
                _ioController.CountedItems = psAPI.GetAllItems();
            }
            dataGridMain.ItemsSource = _ioController.CountedItems;
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
            EditCycle EditCycle = new EditCycle(Controller);
            EditCycle.Show();

        }

        private void showLog_Click(object sender, RoutedEventArgs e)
        {
            

            LogWindow logWindow = new LogWindow(_ioController.Log);
            logWindow.Show();
        }

        private void finishCycle_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("ARE YOU SURE?", "Finish the cycle?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
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
            //Server.ShutdownServer();

            _ioController.Save();
            Application.Current.Shutdown();
        }

        private void comboBoxChooseGet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //e.AddedItems[0].ToString()
        }

        private void changeLanguage_Click(object sender, RoutedEventArgs e) {
            var danishCultureInfo = new CultureInfo("da-DK", true);
            var englishCultureInfo = new CultureInfo("en-GB", true);

            if(CultureInfo.CurrentUICulture.Name == danishCultureInfo.Name) {
                CultureInfo.CurrentUICulture = englishCultureInfo;
            } else {
                CultureInfo.CurrentUICulture = danishCultureInfo;
            }

            Application.Current.MainWindow.UpdateLayout();
        }

    }
}
