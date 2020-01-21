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
using Model.Log;

namespace WPF_PC {
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window {
        public LogWindow(LogFile log) {
            InitializeComponent();
            _log = log;
            PrepareWindow();
            datePickerAfter.Focus();
        }
        public LogWindow(string logPath) {
            InitializeComponent();
            _log = new LogReader().GetLogFromFile(logPath);
            PrepareWindow();
        }

        private LogFile _log;

        private void PrepareWindow() {
            // Prepare datagrid
            _dataGrid.ItemsSource = _log.Messages;

            // Prepare Datepickers
            datePickerAfter.Text = _log.StartDate.ToString("yyyy/MM/dd HH:mm:ss");
            datePickerBefore.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e) {
            Search();
        }

        private void Search() {
            _dataGrid.ItemsSource = _log.Search(GetFilter());
        }

        private LogSearchFilter GetFilter() {
            return new LogSearchFilter(DateTime.Parse(
                datePickerAfter.Text), 
                DateTime.Parse(datePickerBefore.Text), 
                textboxUser.Text, 
                textboxLocation.Text, 
                textboxItem.Text
                );
        }

        private void EnterKeySearch(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                Search();
            }
        }
    }
}
