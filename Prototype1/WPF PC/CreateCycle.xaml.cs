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
using Model;

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
        }

        private void CreateCycleCountButton_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxChooseCycleCountTypes.SelectedIndex == -1 || TextBoxCycleName.Text == "")
            {
                labelWarningTwo.Visibility = Visibility.Visible;
            }
            else
            {
                labelWarningTwo.Visibility = Visibility.Hidden;

                //Cycle newCycle = new Cycle(TextBoxCycleName.Text);

                this.Close();
            }
        }
    }
}
