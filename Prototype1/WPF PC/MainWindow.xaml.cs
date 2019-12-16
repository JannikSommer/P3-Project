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
        private Controller Controller { get; set; } = new Controller();
        private MainController MainController { get; set; }

        public EditCycle EditCycleWindow;


        public MainWindow()
        {
            EditCycleWindow = new EditCycle(Controller);
            MainController = new MainController(Controller);


            InitializeComponent();


            // MainController.StartServer();
            UpdateAllUI();
            LoadIntoDataGrid();
            LoadIntoComboBox();
            Controller.Cycle.Log.AddMessage(new VerificationLogMessage(DateTime.Now, "Bob", "564738920", true));
            if (MainController.Status.IsInitialized == true)
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
            if (MainController.Status.IsInitialized == true)
            {
                dataGridMain.ItemsSource = MainController.Status.CountedItems;
            }
            else
            {
                dataGridMain.ItemsSource = Controller.Cycle.CountedItems;
            }
        }

        public void UpdateMainWindow()
        {
            if (MainController.Status.IsInitialized == true)
            {
                //Active Clients:
                acticeClients.Content;

                //Counted Items overview:
                double countedInt = Controller.Cycle.CountedItems.Count;
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
            else
            {
                //Active Clients:
                acticeClients.Content = MainController.Controller.Active_Clients.Count;

                //Counted Items overview:
                double countedInt = Controller.Cycle.CountedItems.Count;
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
        }

        #region New windows 
        private void CreateCycleCount_Click(object sender, RoutedEventArgs e) {
            CreateCycleWindow CreateCycle = new CreateCycleWindow(Controller);
            CreateCycle.Show();
        }

        private void EditCycle_Click(object sender, RoutedEventArgs e) {
            EditCycle EditCycle = new EditCycle(Controller);
            EditCycle.Show();
        }

        private void ShowLog_Click(object sender, RoutedEventArgs e) {
            LogWindow logWindow = new LogWindow(Controller.Cycle.Log);
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
            MainController.Status.SaveProgressToFile();
            // MainController.ServerShutdown();
            new IOController(Controller.Cycle.Id).Save(Controller.Cycle, Controller.Location_Comparer.ShelfHierarchy);
            Application.Current.Shutdown();
        }

        private void ComboBoxDataSelection_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            if(ComboBoxDataSelection.SelectedIndex == 0) 
            { //Todays Counted
                dataGridMain.ItemsSource = Controller.Cycle.CountedItems;
            }
            else if(ComboBoxDataSelection.SelectedIndex == 1) 
            { //Counted in this cycle
                List<Item> newList = new List<Item>(Controller.Cycle.CountedItems);
                newList.AddRange(Controller.Cycle.VerifiedItems);
                dataGridMain.ItemsSource = newList;
            }
            else if(ComboBoxDataSelection.SelectedIndex == 2) 
            {
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
            MainController.InitializeStatus();
            initializeStatusButton.IsEnabled = false;
            endStatusButton.IsEnabled = true;
        }

        private void EndStatus_Click(object sender, RoutedEventArgs e)
        {
            if (MainController.Status.NotCountedItems.Count > 0)
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
                        MainController.Status.FinishStatus();
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
                    MainController.Status.FinishStatus();
                }
            }
        }
    }
}
