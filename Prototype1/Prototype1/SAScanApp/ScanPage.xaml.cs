using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace SAScanApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {

        public Model.Partition _partition { get; set; }
        public List<Model.Location> _locationList { get; set; }
        public List<Model.Item> _itemList { get; set; }

        bool lightOn = false;

        public ScanPage()
        {
            InitializeComponent();
        }

        public ScanPage(Model.Partition partition)
            : this()
        {
            _partition = partition;
            displayList.ItemsSource = partition.Locations;
            DependencyService.Get<IBluetoothHandler>().enableBluetooth();

        }



        private async void Menu_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuStartPage(this));
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
            DependencyService.Get<IBluetoothHandler>().getPairedDevices();
        }

        private void displayList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            displayList.SelectedItem = null;
        }

        private async void displayList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new LocationSelected(this, _partition.Locations[1].Items));
        }
    }
}