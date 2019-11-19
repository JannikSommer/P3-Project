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

namespace WPF_PC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public class Item
        {
            public string itemID { get; set; }
            public string itemName { get; set; }
            public string itemLocation { get; set; }
            public bool itemHasBeenCounted { get; set; }
            public int itemInStorageCount { get; set; }
            public int itemServerCount { get; set; }
            public int itemCountVariation { get; set; }
        }

        public List<string> settings = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            settings.Add("I dags optalte");
            settings.Add("Optalte i denne cyklus");
            settings.Add("Optalte med varians");

            comboBoxChooseGet.ItemsSource = settings;

            Item itemOne = new Item();

            itemOne.itemID = "12345";
            itemOne.itemName = "Hvid T-Shirt";
            itemOne.itemLocation = "001E02, 001F02";
            itemOne.itemHasBeenCounted = true;
            itemOne.itemInStorageCount = 34;
            itemOne.itemServerCount = 35;
            itemOne.itemCountVariation = 1;

            dataGridMain.Items.Add(itemOne);
        }

        private void createCycleCount_Click(object sender, RoutedEventArgs e)
        {
            Window1 CreateCycle = new Window1();
            CreateCycle.Show();

        }

        private void showLog_Click(object sender, RoutedEventArgs e)
        {

        }

        private void updateServer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void showChosenType_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxChooseGet.SelectedIndex == -1)
            {
                labelWarning.Visibility = Visibility.Visible;
            }
            else if (comboBoxChooseGet.SelectedIndex == 0)
            {
                labelWarning.Visibility = Visibility.Hidden;

            }
            else if (comboBoxChooseGet.SelectedIndex == 1)
            {
                labelWarning.Visibility = Visibility.Hidden;

            }
            else if (comboBoxChooseGet.SelectedIndex == 2)
            {
                labelWarning.Visibility = Visibility.Hidden;

            }
        }

        private void comboBoxChooseGet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //e.AddedItems[0].ToString()
        }
    }
}
