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

namespace WPF_PC
{
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

        private Thread NetworkingThread; // Used to keep socket connection open for clients. 

        int numberOfTimesChangeLanguageIsPressed = 0;

        public MainWindow()
        {
            InitializeComponent();

            StartServer();

            UpdateMainWindow();

            LoadIntoDataGrid();
            LoadIntoChooseBox();
        }

        private void StartServer()
        {
            Server server = new Server();
            NetworkingThread = new Thread(new ThreadStart(server.StartServer));
            NetworkingThread.Start();
        }

        public void LoadIntoDataGrid()
        {
            Item itemOne = new Item();

            itemOne.itemID = "12345";
            itemOne.itemName = "Hvid T-Shirt";
            itemOne.itemLocation = "001E002, 001F002";
            itemOne.itemHasBeenCounted = true;
            itemOne.itemInStorageCount = 34;
            itemOne.itemServerCount = 35;
            itemOne.itemCountVariation = 1;

            Item itemTwo = new Item();
            itemTwo.itemID = "12344";
            itemTwo.itemName = "Sort T-Shirt";
            itemTwo.itemLocation = "001E003, 001F003";
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
            int activeClientsInt = 1;

            acticeClients.Content = (activeClientsInt);

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
            Window1 CreateCycle = new Window1();
            CreateCycle.Show();

        }

        private void editCycle_Click(object sender, RoutedEventArgs e)
        {
            EditCycle EditCycle = new EditCycle();
            EditCycle.Show();

        }

        private void showLog_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void finishCycle_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("ER DU SIKKER?", "Færdiggør Cyklus'en?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
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
            List<string> settings = new List<string>();

            settings.Add("I dags optalte");
            settings.Add("Optalte i denne cyklus");
            settings.Add("Optalte med difference");

            comboBoxChooseGet.ItemsSource = settings;

        }

        private void showChosenType_Click(object sender, RoutedEventArgs e)
        {
            string optionChosen = null;

            if (comboBoxChooseGet.SelectedIndex == -1)
            {
                labelWarning.Visibility = Visibility.Visible;
            }
            else
            {
                labelWarning.Visibility = Visibility.Hidden;

                optionChosen = comboBoxChooseGet.SelectedItem.ToString();

                if (optionChosen == "I dags optalte")
                {

                }
                else if(optionChosen == "Optalte i denne cyklus")
                {

                }
                else if (optionChosen == "Optalte med difference")
                {

                }
            }
            //Loading load = new Loading();
            //load.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void comboBoxChooseGet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //e.AddedItems[0].ToString()
        }

        //Change Language configurations:

        private void changeLanguage_Click(object sender, RoutedEventArgs e)
        {
            numberOfTimesChangeLanguageIsPressed++;
            int languageIndex = (numberOfTimesChangeLanguageIsPressed % 2);

            // Create Cycle Count Window
            Window1.changeLanguageCreateCycle(languageIndex);



            //Danish:

            if (languageIndex == 0)
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

                // Create Cycle Count Window




            }

            //English:

            else if (languageIndex == 1)
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
