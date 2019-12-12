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
using Central_Controller;

namespace WPF_PC {
    public partial class CreateCycleWindow : Window {
        private Controller _controller;

        public CreateCycleWindow(Controller controller) {
            InitializeComponent();
            _controller = controller;
        }

        private void CreateCycleCountButton_Click(object sender, RoutedEventArgs e) {
            if(MessageBox.Show(Localization.Resources.CreateCycleMsgBoxText, Localization.Resources.CreateCycleMsgBoxTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) {
                return;
            }

            if (comboBoxChooseCycleCountTypes.SelectedIndex == -1 || TextBoxCycleName.Text == "") {
                labelWarningTwo.Visibility = Visibility.Visible;
            } else {
                labelWarningTwo.Visibility = Visibility.Hidden;
                Cycle result = new Cycle(TextBoxCycleName.Text) { AllItems = _controller.Cycle.AllItems};
                _controller.Cycle = result;
                Close();
            }
        }
    }
}
