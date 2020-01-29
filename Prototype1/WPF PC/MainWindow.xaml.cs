using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Central_Controller.Central_Controller;
using System.Linq;
using Model;
using Central_Controller.IO;
using System.Globalization;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading;
using Networking;

namespace WPF_PC {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            Controller = new Controller();
            _server = new Server(Controller);
            _networkingThread = new Thread(_server.StartServer);
            _networkingThread.Start();
            
            LoadIntoDataGrid();
            LoadIntoComboBox();
            // Controller.PropertyChanged += UpdateActiveUser;
            // Controller.Cycle.PropertyChanged += UpdatePercentageCounted;
            // Controller.Cycle.PropertyChanged += UpdatePercentageCountedDifference;
        }

        public Controller Controller { get; set; }
        private Server _server;
        private Thread _networkingThread;

        public void LoadIntoDataGrid() {
            // dataGridMain.ItemsSource = Controller.Cycle.CountedItems;
        }

        #region Statistics

        public void UpdateActiveUser(object sender, PropertyChangedEventArgs e)
        {
            acticeClients.Content = ((Controller)sender).NumberOfActiveUsers;
        }

        public void UpdatePercentageCounted(object sender, PropertyChangedEventArgs e)
        {
            if (Controller.Cycle.NumberOfCountedItems != 0 && Controller.Cycle.NumberOfItems != 0)
            {
                overviewTotalCounted.Content = Controller.Cycle.NumberOfCountedItems + " / " + Controller.Cycle.NumberOfItems + "   (" + Math.Round(((double)Controller.Cycle.NumberOfCountedItems / (double)Controller.Cycle.NumberOfItems) * 100, 1) + "%)";
            }
        }

        public void UpdatePercentageCountedDifference(object sender, PropertyChangedEventArgs e)
        {
            if (Controller.Cycle.NumberOfItems != 0)
            {
                double countedIntWithDifference = (from item in (List<Item>)dataGridMain.ItemsSource where item.QuantityVariance != 0 select (Item)item).Count();
                overviewTotalCountedWithDifference.Content = countedIntWithDifference + " / " + Controller.Cycle.NumberOfItems + "   (" + Math.Round(countedIntWithDifference / (double)Controller.Cycle.NumberOfItems * 100, 1) + "%)";
            }
        }

        #endregion

        #region Open New Windows


        //Makes sure that only one window is opened, if not open a new.
        private void CreateCycleCount_Click(object sender, RoutedEventArgs e)
        {

            bool isItOpened = false;

            foreach (Window openedWindows in Application.Current.Windows)
            {
                if (openedWindows.Name == "CreateCycle")
                {
                    isItOpened = true;
                    openedWindows.Activate();
                    break;
                }
            }

            if (isItOpened == false)
            {
                CreateCycleWindow createCycle = new CreateCycleWindow(Controller);
                createCycle.Show();
            }
        }

        //Makes sure that only one window is opened, if not open a new.
        private void EditCycle_Click(object sender, RoutedEventArgs e)
        {

            bool isItOpened = false;

            foreach (Window openedWindows in Application.Current.Windows)
            {
                if (openedWindows.Name == "EditCycle1")
                {
                    isItOpened = true;
                    openedWindows.Activate();
                    break;
                }
            }

            if (isItOpened == false)
            {
                EditCycle editCycleWindow = new EditCycle(Controller);
                editCycleWindow.Show();
            }
        }

        //Makes sure that only one window is opened, if not open a new.
        private void ShowLog_Click(object sender, RoutedEventArgs e)
        {

            bool isItOpened = false;

            foreach (Window openedWindows in Application.Current.Windows)
            {
                if (openedWindows.Name == "LogWindow1")
                {
                    isItOpened = true;
                    openedWindows.Activate();
                    break;
                }
            }

            if (isItOpened == false)
            {
                LogWindow logWindow1 = new LogWindow(Controller.Cycle.Log);
                logWindow1.Show();
            }
        }

        #endregion

        public void LoadIntoComboBox() { // TODO: Move to XAML
            List<string> settings = new List<string> { 
                Localization.Resources.MainWindowComboboxCountedToday, 
                Localization.Resources.MainWindowComboboxCountedThisCycle, 
                Localization.Resources.MainWindowComboboxCountedDifference
            };
           
            ComboBoxDataSelection.ItemsSource = settings;
            ComboBoxDataSelection.SelectedIndex = 0;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            new IOController(Controller.Cycle.Id).Save(Controller.Cycle, Controller.Location_Comparer.ShelfHierarchy);
            Application.Current.Shutdown();
        }

        private void ComboBoxDataSelection_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if(ComboBoxDataSelection.SelectedIndex == 0) { //Todays Counted
                dataGridMain.ItemsSource = Controller.Cycle.CountedItems;
            } else if(ComboBoxDataSelection.SelectedIndex == 1) { //Counted in this cycle
                List<Item> newList = new List<Item>(Controller.Cycle.CountedItems);
                newList.AddRange(Controller.Cycle.VerifiedItems);
                dataGridMain.ItemsSource = newList;
            } else if(ComboBoxDataSelection.SelectedIndex == 2) { //Counted with difference
                // TODO: Missing behavior
            }
        }

        #region Obsolete

        public void UpdateStatistics()
        {
            double percentageCounted = 0,
                   countedIntWithDifference = 0,
                   percentageCountedWithDifference = 0;

            //Active Users:
            //acticeClients.Content = TheController.ActiveUsers.Count;

            //Counted Items overview: 
            if (Controller.Cycle.NumberOfCountedItems != 0 && Controller.Cycle.NumberOfItems != 0)
            {
                percentageCounted = Math.Round((double)(Controller.Cycle.NumberOfCountedItems / Controller.Cycle.NumberOfItems) * 100, 1);
            }

            //Counted with difference overview:
            if (Controller.Cycle.NumberOfItems != 0)
            {
                countedIntWithDifference = (from item in (List<Item>)dataGridMain.ItemsSource where item.QuantityVariance != 0 select (Item)item).Count();
                percentageCountedWithDifference = Math.Round(countedIntWithDifference / Controller.Cycle.NumberOfItems * 100, 1);
            }

            overviewTotalCountedWithDifference.Content = (countedIntWithDifference + " / " + Controller.Cycle.NumberOfItems + "   (" + percentageCountedWithDifference + "%)");
            overviewTotalCounted.Content = Controller.Cycle.NumberOfCountedItems + " / " + Controller.Cycle.NumberOfItems + "   (" + percentageCounted + "%)";
        }

        #endregion

    }
}
