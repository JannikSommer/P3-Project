using SAScanApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Model;

namespace SAScanApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BS_StartPage : ContentPage
    {
        public Model.Partition _partition { get; set; }
        public List<Model.Location> _locationList { get; set; }
        public List<Model.Item> _itemList { get; set; }
        
        bool lightOn = false;
        
        public BS_StartPage()
        {     
            InitializeComponent();       
        }

        public BS_StartPage(Model.Partition partition)
            :this()
        {
            _partition = partition;
           displayList.ItemsSource = partition.Locations;

        }

        

        private async void Menu_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new M_StartPage(this));
        }

        private async void Light_Button_Clicked(object sender, EventArgs e)
        {
            
            try
            {
                if (lightOn == false)
                {
                    // Turn On Flashlight  
                    await Flashlight.TurnOnAsync();
                    lightOn = true;
                }

                else if (lightOn == true)
                {
                    await Flashlight.TurnOffAsync();
                    lightOn = false;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Error:", "Your device does not support the use of this feature not supported, sorry!", "Okay");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Error:", "Enable permissions to access flashlight in your phone", "Okay");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "There has been an error", "Okay");
            }
        }
        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Test", "Test", "Test");
        }

        private void displayList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
            displayList.SelectedItem = null;
        }

        private async void displayList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
             await Navigation.PushAsync(new BS_LocationSelected(this, _partition.Locations.Items));
        }
    }
}