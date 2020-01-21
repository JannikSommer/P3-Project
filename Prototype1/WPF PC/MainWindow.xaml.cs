using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Networking;
using System.Threading;
using Model.Log;
using Localization;
using Central_Controller;
using Model;
using Central_Controller.IO;
using System.Globalization;
using PrestaSharpAPI;
using MVC_Controller;
using StatusController;


namespace WPF_PC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Controller CycleController { get; set; } = new Controller();
        public Status StatusController { get; set; } = new Status();
        private Thread NetworkingThread { get; set; }
        private Server Server { get; set; }

        public EditCycle EditCycleWindow;


        public MainWindow()
        {
            EditCycleWindow = new EditCycle(CycleController);
            //MainController = new MainController(Controller);
            ////StartServer();
            InitializeComponent();


            // MainController.StartServer();
            LoadIntoDataGrid();
            LoadIntoComboBox();
            CycleController.Cycle.Log.AddMessage(new VerificationLogMessage(DateTime.Now, "Bob", "564738920", true));
            if (StatusController.IsInitialized == true)
            {
                initializeStatusButton.IsEnabled = false;
                endStatusButton.IsEnabled = true;
            }
        }

        public void UpdateAllUI()
        {
            UpdateMainWindow();
            LoadIntoDataGrid();
        }

        public void LoadIntoDataGrid() 
        {
            if (StatusController.IsInitialized == true)
            {
                dataGridMain.ItemsSource = StatusController.CountedItems;
            }
            else
            {
                dataGridMain.ItemsSource = CycleController.Cycle.CountedItems;
            }
        }



        public void UpdateMainWindow()
        {
            if (StatusController.IsInitialized == true)
            {
                //Active Clients:
                acticeClients.Content = "N/A"; // No implementation for showing number of clients connected

                //Counted Items overview:
                double countedInt = StatusController.CountedItems.Count;
                double totalItemsInt = StatusController.ServerItems.Count;
                double percentageCounted = ((countedInt / totalItemsInt) * 100);
                double percentageCountedRoundedDown = Math.Round(percentageCounted, 1);

                overviewTotalCounted.Content = (countedInt + " / " + totalItemsInt + "   (" + percentageCountedRoundedDown + "%)");

                //Counted with difference overview:
                double countedIntWithDifference = 0; // Not supported for Status
                double percentageCountedWithDifference = 0; // ((countedIntWithDifference / totalItemsInt) * 100); ---- Not supported for status
                double percentageCountedWithDifferenceRoundedDown = 0;// Math.Round(percentageCountedWithDifference, 1); ----- not supported for status

                overviewTotalCountedWithDifference.Content = 0; // (countedIntWithDifference + " / " + totalItemsInt + "   (" + percentageCountedWithDifferenceRoundedDown + "%)"); ----- not supported for status
            }
            else
            {
                //Active Clients:
                acticeClients.Content = CycleController.Active_Clients.Count;

                //Counted Items overview:
                double countedInt = CycleController.Cycle.CountedItems.Count;
                double totalItemsInt = CycleController.Cycle.AllItems.Count;
                double percentageCounted = ((countedInt / totalItemsInt) * 100);
                double percentageCountedRoundedDown = Math.Round(percentageCounted, 1);

                overviewTotalCounted.Content = (countedInt + " / " + totalItemsInt + "   (" + percentageCountedRoundedDown + "%)");

                //Counted with difference overview: HOW THE FUCK?
                double countedIntWithDifference = 340; 
                double percentageCountedWithDifference = ((countedIntWithDifference / totalItemsInt) * 100);
                double percentageCountedWithDifferenceRoundedDown = Math.Round(percentageCountedWithDifference, 1);

                overviewTotalCountedWithDifference.Content = (countedIntWithDifference + " / " + totalItemsInt + "   (" + percentageCountedWithDifferenceRoundedDown + "%)");
            }
        }

        #region New windows 
        private void CreateCycleCount_Click(object sender, RoutedEventArgs e) {
            CreateCycleWindow CreateCycle = new CreateCycleWindow(CycleController);
            CreateCycle.Show();
        }

        private void EditCycle_Click(object sender, RoutedEventArgs e) {
            EditCycle EditCycle = new EditCycle(CycleController);
            EditCycle.Show();
        }

        private void ShowLog_Click(object sender, RoutedEventArgs e) {
            LogWindow logWindow = new LogWindow(CycleController.Cycle.Log);
            logWindow.Show();
        }

        public void LoadIntoComboBox() {
            List<string> settings = new List<string> { 
                Localization.Resources.MainWindowComboboxCountedToday, 
                Localization.Resources.MainWindowComboboxCountedThisCycle, 
                Localization.Resources.MainWindowComboboxCountedDifference
            };
           
            ComboBoxDataSelection.ItemsSource = settings;
            ComboBoxDataSelection.SelectedIndex = 0;
        }
        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) 
        {
            StatusController.SaveProgressToFile();
            // MainController.ServerShutdown();
            new IOController(CycleController.Cycle.Id).Save(CycleController.Cycle, CycleController.Location_Comparer.ShelfHierarchy);
            Application.Current.Shutdown();
        }

        private void ComboBoxDataSelection_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            if(ComboBoxDataSelection.SelectedIndex == 0) 
            { //Todays Counted
                if (StatusController.IsInitialized == true)
                {

                }
                else
                {
                    dataGridMain.ItemsSource = CycleController.Cycle.CountedItems;
                }
            }
            else if (ComboBoxDataSelection.SelectedIndex == 1) 
            { //Counted in this cycle
                if (StatusController.IsInitialized == true)
                {

                }
                else
                {
                    List<Item> newList = new List<Item>(CycleController.Cycle.CountedItems);
                    newList.AddRange(CycleController.Cycle.VerifiedItems);
                    dataGridMain.ItemsSource = newList;
                }
            }
            else if (ComboBoxDataSelection.SelectedIndex == 2) 
            {
                if (StatusController.IsInitialized == true)
                {

                }
                else
                {
                    
                }
                //Counted with difference
            }
        }

        private void ChangeLanguage_Click(object sender, RoutedEventArgs e) 
        {
            var danishCultureInfo = new CultureInfo("da-DK", true);
            var englishCultureInfo = new CultureInfo("en-GB", true);

            if(CultureInfo.CurrentUICulture.Name == danishCultureInfo.Name) 
            {
                CultureInfo.CurrentUICulture = englishCultureInfo;
            } 
            else 
            {
                CultureInfo.CurrentUICulture = danishCultureInfo;
            }
            Application.Current.MainWindow.UpdateLayout();
        }

        private void InitializeStatusButton_Click(object sender, RoutedEventArgs e)
        {
            // MainController.InitializeStatus();
            dataGridMain.ItemsSource = StatusController.CountedItems;
            initializeStatusButton.IsEnabled = false;
            endStatusButton.IsEnabled = true;
            StatusController.StartStatus();

        }

        private void EndStatus_Click(object sender, RoutedEventArgs e)
        {
            if (StatusController.NotCountedItems.Count > 0)
            {
                if (MessageBox.Show("Not all items has been acounted for. Are you sure every location has been scanned?", "WARNING!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    // Nothing happens.
                }
                else
                {
                    if (MessageBox.Show("This action cannot be undone. Are you sure you want to end status?", "WARNING!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        // Nothing
                    }
                    else
                    {
                        StatusController.FinishStatus();
                    }
                }
            }
            else
            {
                if (MessageBox.Show("This action cannot be undone. Are you sure you want to end status?", "WARNING!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    // Nothing
                }
                else
                {
                    StatusController.FinishStatus();
                }
            }
        }

        public void StartServer()
        {
            Server = new Server(CycleController, StatusController);
            NetworkingThread = new Thread(new ThreadStart(Server.StartServer));
            NetworkingThread.Start();
        }

        public void ServerShutdown()
        {
            Server.ShutdownServer();
            NetworkingThread.Abort();
        }
    }
}
