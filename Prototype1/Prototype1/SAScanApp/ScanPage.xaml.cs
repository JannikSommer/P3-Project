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
        public ObservableCollection<Model.Location> LocationList { get; set; }
        public List<Item> ItemList { get; set; }
        public Partition Partition { get; set; }
        public VerificationPartition VerificationPartition { get; set; }
        private string _userName { get; set; }

        private bool lightOn = false;

        public ScanPage(Partition partition) {
            Partition = partition;
            InitializeComponent();
            LocationList = new ObservableCollection<Model.Location>();

            for (int i = 0; i < partition.Locations.Count; i++) {
                LocationList.Add(partition.Locations[i]);
            }

            LocationList.CollectionChanged += LocationList_CollectionChanged;
            displayList.ItemsSource = LocationList;
        }

        public ScanPage(VerificationPartition VPartition) {
            VerificationPartition = VPartition;
            InitializeComponent();
            LocationList = new ObservableCollection<Model.Location>();          

            for(int i = 0; i < VerificationPartition.Locations.Count; i++) {
                LocationList.Add(VerificationPartition.Locations[i]);
            }

            LocationList.CollectionChanged += LocationList_CollectionChanged;
            displayList.ItemsSource = LocationList;          
        }

        private void LocationList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            displayList.BeginRefresh();
            displayList.EndRefresh();
        }

        private async void Menu_Button_Clicked(object sender, EventArgs e) {
            if(Partition != null) {
                await Navigation.PushAsync(new MenuStartPage(Partition));
            } else {
                await Navigation.PushAsync(new MenuStartPage(VerificationPartition));
            }
        }

        private async void Light_Button_Clicked(object sender, EventArgs e) {
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
                await DisplayAlert("Error:", "This device does not support the use of the flashlight feature", "Okay");
            }
            catch (PermissionException)
            {
                await DisplayAlert("Error:", "Permissions to access flashlight is not enabled on your phone", "Okay");
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "An unknown error has occured", "Okay");
            }
        }

        private void MenuItem_Clicked(object sender, EventArgs e) {
            // Add a Observable Collection List that pops down with the paired devices recieved
            //DependencyService.Get<IBluetoothHandler>().GetPairedDevices();
        }

        private void DisplayList_ItemSelected(object sender, SelectedItemChangedEventArgs e) {
            displayList.SelectedItem = null;
        }

        private async void DisplayList_ItemTapped(object sender, ItemTappedEventArgs e) {
            await Navigation.PushAsync(new LocationSelected(((Model.Location)e.Item).Items, Partition));
        }

        private void ButtonV_Clicked(object sender, EventArgs e) {

        }
    }
}