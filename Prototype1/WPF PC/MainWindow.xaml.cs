using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Central_Controller.Central_Controller;
using System.Linq;
using Model;
using Central_Controller;
using Central_Controller.IO;
using System.Globalization;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading;
using Networking;
using StatusController;

namespace WPF_PC {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            StatusController = new Status();
            _server = new Server(CycleController, StatusController);
            //_networkingThread = new Thread(_server.StartServer);
            //_networkingThread.Start();
            if (StatusController.IsInitialized)
            {
                initializeStatusButton.IsEnabled = false;
                endStatusButton.IsEnabled = true;
            }
            else
            {
                initializeStatusButton.IsEnabled = true;
                endStatusButton.IsEnabled = false;
            }

            LoadIntoDataGrid();
            LoadIntoComboBox();
            UpdateAllUI();
            
            //CycleController.PropertyChanged += UpdateActiveUser;
            //CycleController.Cycle.PropertyChanged += UpdatePercentageCounted;
            //CycleController.Cycle.PropertyChanged += UpdatePercentageCountedDifference;
        }

        public Controller CycleController { get; set; }
        public Status StatusController { get; set; } 
        private Server _server;
        private Thread _networkingThread;


        public void UpdateAllUI() {
            LoadIntoDataGrid();
            UpdateStatistics();          
        }

        public void LoadIntoDataGrid() 
        {
            dataGridMain.ItemsSource = StatusController.CountedItems;
        }


        public void UpdateActiveUser(object sender, PropertyChangedEventArgs e) {
            acticeClients.Content = ((Controller)sender).NumberOfActiveUsers;
        }

        public void UpdatePercentageCounted(object sender, PropertyChangedEventArgs e) {
            if (StatusController.CountedItems.Count == 0)
            {
                int itemNum = StatusController.ServerItems.Count;
                int itemCounted = StatusController.CountedItems.Count;
                double percentage = (itemCounted / itemNum) * 100;
                overviewTotalCounted.Content = Math.Round(percentage);
            }
        }

        public void UpdatePercentageCountedDifference(object sender, PropertyChangedEventArgs e) {
            overviewTotalCountedWithDifference.Content = "N/A";
            //if(CycleController.Cycle.NumberOfItems != 0) {
            //    double countedIntWithDifference = (from item in (List<Item>)dataGridMain.ItemsSource where item.QuantityVariance != 0 select (Item)item).Count();
            //    overviewTotalCountedWithDifference.Content = countedIntWithDifference + " / " + CycleController.Cycle.NumberOfItems + "   (" + Math.Round(countedIntWithDifference / (double)CycleController.Cycle.NumberOfItems * 100, 1) + "%)";
            //}
        }
        public void UpdateStatistics() {
            //double percentageCounted = 0,
            //       countedIntWithDifference = 0,
            //       percentageCountedWithDifference = 0;

            ////Active Users:
            ////acticeClients.Content = TheController.ActiveUsers.Count;

            ////Counted Items overview: 
            //if(CycleController.Cycle.NumberOfCountedItems != 0 && CycleController.Cycle.NumberOfItems != 0) {
            //    percentageCounted = Math.Round((double)(CycleController.Cycle.NumberOfCountedItems / CycleController.Cycle.NumberOfItems) * 100, 1);
            //}

            ////Counted with difference overview:
            //if(CycleController.Cycle.NumberOfItems != 0) {
            //    countedIntWithDifference = (from item in (List<Item>)dataGridMain.ItemsSource where item.QuantityVariance != 0 select (Item)item).Count();
            //    percentageCountedWithDifference = Math.Round(countedIntWithDifference / CycleController.Cycle.NumberOfItems * 100, 1); 
            //}

            //overviewTotalCountedWithDifference.Content = (countedIntWithDifference + " / " + CycleController.Cycle.NumberOfItems + "   (" + percentageCountedWithDifference + "%)");
            if (StatusController.CountedItems.Count != 0 && StatusController.CountedItems != null)
            {
                int itemNum = StatusController.ServerItems.Count;
                int itemCounted = StatusController.CountedItems.Count;
                double percentage = (itemCounted / itemNum) * 100;
                overviewTotalCounted.Content = StatusController.CountedItems.Count + " / " + StatusController.ServerItems.Count + "   (" + percentage + "%)";
            }
            else
                overviewTotalCounted.Content = "0" + " / " + StatusController.ServerItems.Count;

        }

        #region New windows 
        private void CreateCycleCount_Click(object sender, RoutedEventArgs e) {
            new CreateCycleWindow(CycleController).Show();
        }

        private void EditCycle_Click(object sender, RoutedEventArgs e) {
            CycleController.Cycle.NumberOfCountedItems++;
            new EditCycle(CycleController).Show();
        }

        private void ShowLog_Click(object sender, RoutedEventArgs e) {
            CycleController.Cycle.NumberOfItems++;
            new LogWindow(CycleController.Cycle.Log).Show();
        }

        public void LoadIntoComboBox() { // TODO: Move to XAML
            List<string> settings = new List<string> { 
                Localization.Resources.MainWindowComboboxCountedToday, 
                Localization.Resources.MainWindowComboboxCountedThisCycle, 
                Localization.Resources.MainWindowComboboxCountedDifference
            };
           
            ComboBoxDataSelection.ItemsSource = settings;
            ComboBoxDataSelection.SelectedIndex = 0;
        }
        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            // new IOController(CycleController.Cycle.Id).Save(CycleController.Cycle, CycleController.Location_Comparer.ShelfHierarchy);
            _networkingThread.Abort();
            Application.Current.Shutdown();
        }

        private void ComboBoxDataSelection_SelectionChanged(object sender, SelectionChangedEventArgs e) {


            //if(ComboBoxDataSelection.SelectedIndex == 0) { //Todays Counted
            //    dataGridMain.ItemsSource = CycleController.Cycle.CountedItems;
            //} else if(ComboBoxDataSelection.SelectedIndex == 1) { //Counted in this cycle
            //    List<Item> newList = new List<Item>(CycleController.Cycle.CountedItems);
            //    newList.AddRange(CycleController.Cycle.VerifiedItems);
            //    dataGridMain.ItemsSource = newList;
            //} else if(ComboBoxDataSelection.SelectedIndex == 2) { //Counted with difference
            //    // TODO: Missing behavior
            //}
        }

        private void InitializeStatus_Click(object sender, RoutedEventArgs e)
        {
            StatusController.StartStatus();
            initializeStatusButton.IsEnabled = false;
            endStatusButton.IsEnabled = true;
            UpdateStatistics();
        }

        private void EndStatus_Click(object sender, RoutedEventArgs e)
        {
            if (StatusController.CheckForUncountedItems())
            {
                if (MessageBox.Show("Every item has been acounted for. Are you sure you want " +
                                    "to end the status and upload data to database?", "WARNING", 
                                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    StatusController.FinishStatus();
                    MessageBox.Show("The status is completed and database updated.");
                }
                else
                {
                    MessageBox.Show("Data is still stored locally and the database is NOT updated.");
                }
            }
            else
            {
                if (MessageBox.Show("Not every item has been acounted for. Are you sure you want to end the status anyway?", "WARNING",
                                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Items that was accounted for has been updated in the database.");
                    StatusController.FinishStatus();
                }
                else
                {
                    MessageBox.Show("Data is still stored locally and the database is NOT updated.");
                }
            }
        }
    }
}
