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
    /// Interaction logic for EditCycle.xaml
    /// </summary>
    public partial class EditCycle : Window
    {

        public EditCycle()
        {
            InitializeComponent();

            loadAllUsersnamesIntoChooseBox();
            getSortPriority();
        }

        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedindex = listBoxShelfPriority.SelectedIndex;

            //Check if a move up is possible
            if (selectedindex > 0)
            {
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

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedindex = listBoxShelfPriority.SelectedIndex;

            //Check if a move down is possible
            if (selectedindex < listBoxShelfPriority.Items.Count - 1)
            {
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

        public void loadAllUsersnamesIntoChooseBox()
        {
            List<string> settings = new List<string>();

            settings.Add("Bob");
            settings.Add("Svend");
            settings.Add("Jørgen");

            comboBoxChooseEdit.ItemsSource = settings;
        }

        public void getSortPriority()
        {
            List<string> sortPriority = new List<string>();

            for (int index = 0; index < listBoxShelfPriority.Items.Count; index++)
            {
                sortPriority.Add(listBoxShelfPriority.Items.GetItemAt(index).ToString());
            }

            //where to?
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxChooseEdit.SelectedIndex == -1)
            {
                labelWarningNoUserSelected.Visibility = Visibility.Visible;

            }
            else
            {
                labelWarningNoUserSelected.Visibility = Visibility.Hidden;

                string userChosen = comboBoxChooseEdit.SelectedItem.ToString();

                MessageBox.Show(userChosen);


            }
        }

        private void DeleteCycleCountButton_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("ER DU SIKKER?", "Slet Cyklus'en?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
            }
            else
            {
                //do yes stuff

                this.Close();
            }


        }

        private void CancelEdit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmEdit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
