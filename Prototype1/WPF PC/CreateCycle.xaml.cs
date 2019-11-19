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
    public partial class Window1 : Window
    {
        public List<string> createCycleTypes = new List<string>();
        public Window1()
        {
            InitializeComponent();

            createCycleTypes.Add("HELE LAGERET");
            createCycleTypes.Add("T-shirts");
            createCycleTypes.Add("Bukser");
            createCycleTypes.Add("Skateboards");
            createCycleTypes.Add("Sko");
            createCycleTypes.Add("Hoodies");

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
    }
}
