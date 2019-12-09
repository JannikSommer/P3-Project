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
        private Controller Controller { get; set; }
        private Server Server { get; set; }
        //private Thread NetworkingThread; // Used to keep socket connection open for clients. 
        private IOController _ioController = new IOController("TestCycle");


        public MainWindow()
        {
            Controller = new Controller();

            InitializeComponent();
            // StartServer();
            UpdateAllUI();
            LoadIntoDataGrid();
            LoadIntoChooseBox();

            // Add eksample data to save files.
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
            double countedInt = _ioController.CountedItems.Count;
            double totalItemsInt = 80000;
            double percentageCounted = ((countedInt / totalItemsInt) * 100);
            double percentageCountedRoundedDown = Math.Round(percentageCounted, 1);

            overviewTotalCounted.Content = (countedInt + " / " + totalItemsInt + "   (" + percentageCountedRoundedDown + "%)");

            //Counted with difference overview:
            double countedIntWithDifference = 340;
            double percentageCountedWithDifference = ((countedIntWithDifference / totalItemsInt) * 100);
            double percentageCountedWithDifferenceRoundedDown = Math.Round(percentageCountedWithDifference, 1);

            overviewTotalCountedWithDifference.Content = (countedIntWithDifference + " / " + totalItemsInt + "   (" + percentageCountedWithDifferenceRoundedDown + "%)");
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
