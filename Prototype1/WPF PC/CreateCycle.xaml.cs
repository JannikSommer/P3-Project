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
using System.Windows.Shapes;

namespace WPF_PC
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CreateCycleWindow : Window
    {
        public CreateCycleWindow()
        {
            InitializeComponent();

            CreateCycleWindowLanguage();
            LoadIntoChooseBox();
        }

        public void LoadIntoChooseBox()
        {
            List<string> createCycleTypes = new List<string>();

            ////Danish:
            //if (Language.Danish == language)
            //{
            //    createCycleTypes.Add("Hele lageret");
            //    createCycleTypes.Add("T-shirts");
            //    createCycleTypes.Add("Bukser");
            //    createCycleTypes.Add("Skateboards");
            //    createCycleTypes.Add("Sko");
            //    createCycleTypes.Add("Hoodies");
            //}

            ////English:
            //else if (Language.English == language)
            //{
            //    createCycleTypes.Add("The whole storage");
            //    createCycleTypes.Add("T-shirts");
            //    createCycleTypes.Add("Pants");
            //    createCycleTypes.Add("Skateboards");
            //    createCycleTypes.Add("Shoes");
            //    createCycleTypes.Add("Hoodies");
            //}
            comboBoxChooseCycleCountTypes.ItemsSource = createCycleTypes;
        }

        private void CreateCycleCountButton_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxChooseCycleCountTypes.SelectedIndex == -1)
            {
                labelWarningTwo.Visibility = Visibility.Visible;
            }
            else
            {
                labelWarningTwo.Visibility = Visibility.Hidden;

                this.Close();
            }
        }

        public void CreateCycleWindowLanguage()
        {

            //Danish:
            //if (Language.Danish == language)
            //{
            //    //Window name:
            //    CreateCycle.Title = "Opret Optællings Cyklus";

            //    //Labels:
            //    chooseCycle.Content = "Vælg hvordan cyklus'en skal oprettes:";

            //    //Buttons:
            //    CreateCycleCountButton.Content = "Opret cyklus";
            //}

            ////English:
            //else if (Language.English == language)
            //{
            //    //Window name:
            //    CreateCycle.Title = "Create Cycle Count";

            //    //Labels:
            //    chooseCycle.Content = "Choose how to create the cycle:";

            //    //Buttons:
            //    CreateCycleCountButton.Content = "Create cycle";
            //}
        }
    }
}
