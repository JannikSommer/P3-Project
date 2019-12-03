using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using Model;

namespace SAScanApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {

        public ObservableCollection<Model.Location> _locationList { get; set; }
        public List<Item> _itemList { get; set; }

        bool lightOn = false;

        public ScanPage()
        {
            InitializeComponent();
            _locationList = new ObservableCollection<Model.Location>();

            // temp
            _locationList.Add(new Model.Location("001A08",
                                   new List<Item> {
                                   new Item("item1"),
                                   new Item("item2"),
                                   new Item("item3"),
                                   new Item("item4")
                                   }));

            _locationList.CollectionChanged += _locationList_CollectionChanged;
            displayList.ItemsSource = _locationList;
        }

        private void _locationList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            displayList.BeginRefresh();
            displayList.EndRefresh();
        }

        public ScanPage(Partition partition) : this()
        {
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
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Error:", "Your device does not support the use of this feature not supported, sorry!", "Okay");
            }
            catch (PermissionException)
            {
                await DisplayAlert("Error:", "Enable permissions to access flashlight in your phone", "Okay");
            }
            catch (Exception)
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
            await Navigation.PushAsync(new LocationSelected(this, ((Model.Location)e.Item).Items));
        }

        private void ButtonV_Clicked(object sender, EventArgs e) {
            _locationList.Add(
                new Model.Location("003B23",
                                   new List<Item> {
                                   new Item("item1"),
                                   new Item("item2"),
                                   new Item("item3"),
                                   new Item("item4")
                                   }));
        }
    }
}