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
            this.Dispatcher.Invoke(() =>
            {
                _networkingThread = new Thread(_server.StartServer);
                _networkingThread.Start();
            });
            
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

            UpdateAllUI();
            
          
            StatusController.PropertyChanged += UpdatePercentageCounted;
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

        public void UpdatePercentageCounted(object sender, PropertyChangedEventArgs e) {
            if (StatusController.CountedItems.Count == 0)
            {
                int itemNum = StatusController.ServerItems.Count;
                int itemCounted = StatusController.CountedItems.Count;
                double percentage = (itemCounted / itemNum) * 100;
                overviewTotalCounted.Content = Math.Round(percentage);
                LoadIntoDataGrid();
            }
        }

        public void UpdateStatistics() {
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

        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            _networkingThread.Abort();
            //Application.Current.Shutdown();
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
                    initializeStatusButton.IsEnabled = true;
                    endStatusButton.IsEnabled = false;
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
                    initializeStatusButton.IsEnabled = true;
                    endStatusButton.IsEnabled = false;
                }
                else
                {
                    MessageBox.Show("Data is still stored locally and the database is NOT updated.");
                }
            }
        }
    }
}
