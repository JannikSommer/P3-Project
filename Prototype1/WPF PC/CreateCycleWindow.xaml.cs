using System.Windows;
using Model;
using Central_Controller.Central_Controller;

namespace WPF_PC {
    public partial class CreateCycleWindow : Window {
        public CreateCycleWindow(Controller controller) {
            InitializeComponent();
            _controller = controller;
        }

        private Controller _controller;

        private void CreateCycleCountButton_Click(object sender, RoutedEventArgs e) {
            if(MessageBox.Show(Localization.Resources.CreateCycleMsgBoxText, Localization.Resources.CreateCycleMsgBoxTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) {
                return;
            }

            if (comboBoxChooseCycleCountTypes.SelectedIndex == -1 || TextBoxCycleName.Text == "") {
                labelWarningTwo.Visibility = Visibility.Visible;
            } else {
                labelWarningTwo.Visibility = Visibility.Hidden;
                _controller.Cycle = new Cycle(TextBoxCycleName.Text) { AllItems = _controller.Cycle.AllItems};
                _controller.Location_Comparer = new LocationComparer(19);    
                Close();
            }
        }
    }
}
