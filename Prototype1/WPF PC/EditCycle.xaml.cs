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
        private new Language Language;
        public EditCycle(Language language)
        {
            InitializeComponent();

            EditCycleWindowLanguage(language);
            loadAllUsersnamesIntoChooseBox();
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
            if (selectedindex < listBoxShelfPriority.Items.Count - 1 && listBoxShelfPriority.SelectedItem != null)
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
                sortPriority.Add(((ListViewItem)listBoxShelfPriority.Items.GetItemAt(index)).Content.ToString());
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
            getSortPriority();
            this.Close();
        }

        public void EditCycleWindowLanguage(Language language)
        {

            //Danish:
            if (Language.Danish == language)
            {
                //Labels:
                chooseHowToEditCycle.Content = "Vælg hvordan Cyklus'en skal Redigeres:";
                chooseThePriority.Content = "Vælg hvordan reolerne skal sorteres:";
                chooseUsers.Content = "Slet en specifik brugers arbejde:";
                chooseToDeleteTheWholeCycle.Content = "Slet hele cyklus'en:";

                //Buttons:
                MoveUpButton.Content = "Flyt op";
                MoveDownButton.Content = "Flyt ned";
                DeleteUserButton.Content = "Slet brugers arbejde";
                DeleteCycleCountButton.Content = "Slet Cyklus";
                ConfirmEdit.Content = "OK";
                CancelEdit.Content = "Annuller";
            }

            //English:
            else if (Language.English == language)
            {
                //Labels:
                chooseHowToEditCycle.Content = "Choose how to edit the cycle:";
                chooseThePriority.Content = "Choose how to sort the shelfs:";
                chooseUsers.Content = "Delete a specific users work:";
                chooseToDeleteTheWholeCycle.Content = "Delete the whole cycle:";

                //Buttons:
                MoveUpButton.Content = "Move up";
                MoveDownButton.Content = "Move down";
                DeleteUserButton.Content = "Delete users work";
                DeleteCycleCountButton.Content = "Delete cycle";
                ConfirmEdit.Content = "OK";
                CancelEdit.Content = "Cancel";
            }
        }
    }
}
