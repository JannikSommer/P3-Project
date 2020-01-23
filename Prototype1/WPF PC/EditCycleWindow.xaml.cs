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
using Central_Controller.IO;
using System.IO;
using Central_Controller;
using System.Text.RegularExpressions;
using Central_Controller.Central_Controller;

namespace WPF_PC {
    /// <summary>
    /// Interaction logic for EditCycle.xaml
    /// </summary>
    public partial class EditCycle : Window {
        public EditCycle(Controller controller) {
            InitializeComponent();
            _controller = controller;
            comboBoxChooseEdit.ItemsSource = _controller.ActiveUsers;
            _shelfArray = controller.Location_Comparer.ShelfHierarchy;
            //listBoxShelfPriority.ItemsSource = _shelfArray;
            LoadSortPriority();
        }

        private int[] _shelfArray;
        private Controller _controller;


        private void MoveUpButton_Click(object sender, RoutedEventArgs e) {
            int selectedindex = listBoxShelfPriority.SelectedIndex;

            //Check if a move up is possible
            if (selectedindex > 0) {
                //Copy the selected
                var item = new ListViewItem();
                string text = ((ListViewItem)listBoxShelfPriority.SelectedItem).Content.ToString();
                item.Content = text;

                //Insert the selected
                listBoxShelfPriority.Items.Insert(listBoxShelfPriority.SelectedIndex - 1, item);

                //Remove the first selected item
                listBoxShelfPriority.Items.RemoveAt(listBoxShelfPriority.SelectedIndex);

                //Select index
                listBoxShelfPriority.SelectedIndex = selectedindex - 1;
            }
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e) {
            int selectedindex = listBoxShelfPriority.SelectedIndex;
            
            //Check if a move down is possible
            if (selectedindex < listBoxShelfPriority.Items.Count - 1 && listBoxShelfPriority.SelectedItem != null) {
                //Copy the selected
                var item = new ListViewItem();
                string text = ((ListViewItem)listBoxShelfPriority.SelectedItem).Content.ToString();
                item.Content = text;

                //Insert the selected
                listBoxShelfPriority.Items.Insert(listBoxShelfPriority.SelectedIndex + 2, item);

                //Remove the first selected item
                listBoxShelfPriority.Items.RemoveAt(listBoxShelfPriority.SelectedIndex);

                //Select index
                listBoxShelfPriority.SelectedIndex = selectedindex + 1;
            }
        }

        [Obsolete]
        public void SaveSortPriority() {
            //Get sorting priority and load into list.
            int index;
            List<string> sortPriority = new List<string>();

            for (index = 0; index < listBoxShelfPriority.Items.Count; index++) {
                sortPriority.Add(((ListViewItem)listBoxShelfPriority.Items.GetItemAt(index)).Content.ToString());
            }

            //load into text file.
            string filepath = Environment.CurrentDirectory + @"\SortPriority.txt";
            StringBuilder priority = new StringBuilder();
            index = 0;

            foreach(string shelf in sortPriority) {
                if(index < listBoxShelfPriority.Items.Count - 1) {
                    priority.Append(shelf + ",");
                } else if(index == listBoxShelfPriority.Items.Count - 1) {
                    priority.Append(shelf);
                }
                index++;
            }

            File.WriteAllText(filepath, priority.ToString());
        }

        [Obsolete]
        public int[] RetrieveSortingPriorityFromFile() {
            int[] priority = new int[0];
            string filePath = Environment.CurrentDirectory + @"\SortPriority.txt";
            if (File.Exists(filePath)) {
                List<string> lines = File.ReadAllLines(filePath).ToList();
                foreach (string line in lines) {
                    priority = Array.ConvertAll(line.Split(','), int.Parse);
                }
                return priority;
            } else {
                return priority;
            }
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e) {
            if(comboBoxChooseEdit.SelectedIndex == -1) {
                labelWarningNoUserSelected.Visibility = Visibility.Visible;
            } else {
                labelWarningNoUserSelected.Visibility = Visibility.Hidden;
                string userChosen = comboBoxChooseEdit.SelectedItem.ToString();
                // TODO: Undo this users changes or something
            }
        }

        [Obsolete]
        private void DeleteCycleCountButton_Click(object sender, RoutedEventArgs e) {
            if(MessageBox.Show("ER DU SIKKER?", "Slet Cyklus'en?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) {
                //do no stuff
            } else {
                //do yes stuff
                Close();
            }
        }

        private void LoadSortPriority() {

            if(new IOController().LoadShelves() != null)
            {
                ListViewItem item;
                for (int x = 0; x < _shelfArray.Length; x++)
                {
                    item = new ListViewItem
                    {
                        Content = _shelfArray[x].ToString()
                    };
                    listBoxShelfPriority.Items.Add(item);
                }
            }
        }

        private void ConfirmEdit_Click(object sender, RoutedEventArgs e) {
            new IOController().SaveSortpriority(_shelfArray);
            Close();
        }

        private void CancelEdit_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void ConfirmNumberOfShelfs_Click(object sender, RoutedEventArgs e) {
            ChangeNumberOfShelves();
        }

        //press enter to confirm function
        private void TextBoxNumberofShelfs_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ChangeNumberOfShelves();
            }
        }

        public void ChangeNumberOfShelves() {
            if(TextBoxNumberofShelfs.Text != "")
            {
                ListBoxItem item;
                int numberOfShelfs = Int32.Parse(TextBoxNumberofShelfs.Text);

                listBoxShelfPriority.Items.Clear();

                for (int index = 0; index < numberOfShelfs; index++)
                {
                    item = new ListViewItem();
                    item.Content = index.ToString();
                    listBoxShelfPriority.Items.Add(item);
                }

                _controller.Location_Comparer = new LocationComparer(numberOfShelfs);
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        #region EditShelfNumber

        private void EditContentButton_Click(object sender, RoutedEventArgs e) {
            //Check if anything is selected
            if (listBoxShelfPriority.SelectedIndex != -1) {
                EditShelfNumber();
            }
        }

        private void TextBoxEditShelfNumber_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == System.Windows.Input.Key.Enter) {
                //Check if anything is selected
                if (listBoxShelfPriority.SelectedIndex != -1) {
                    EditShelfNumber();
                }
            }
        }

        public void EditShelfNumber() {
            if (TextBoxEditShelfNumber.Text != "") {
                //Insert the new shelf number.
                ((ListViewItem)listBoxShelfPriority.SelectedItem).Content = TextBoxEditShelfNumber.Text;
                TextBoxEditShelfNumber.Clear();
            }
        }

        #endregion
    }
}
