﻿using System;
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

        public Model.Partition _partition { get; set; }

        bool lightOn = false;

        public ScanPage()
        {
            InitializeComponent();
            DependencyService.Get<IBluetoothHandler>().enableBluetooth();
            Partition _partition = new Partition(new Model.Location("000A01",
                                                                        new List<Item> {
                                                                            new Item("5701872203005"),
                                                                            new Item("64747"),
                                                                            new Item ("8979878"),
                                                                            new Item ("78789"),
                                                                            new Item ("878979")
                                                                            })); 

            ObservableCollection<Model.Location> _locationList = new ObservableCollection<Model.Location>(); // instanser
            
            for (int i = 0; i < _partition.Locations.Count; i++)
            {
                _locationList.Add(_partition.Locations[i]);
            }

            _locationList.CollectionChanged += _locationList_CollectionChanged;
            displayList.ItemsSource = _locationList;
            BarcodeReciever reciever = new BarcodeReciever();
            reciever.RecieveBarcode(_partition);

        }

        public ScanPage(Model.Partition partition)
        {
            _partition = partition;
            
            InitializeComponent();
            _locationList = new ObservableCollection<Model.Location>();          

            // temp
            for(int i = 0; i < partition.Locations.Count; i++)
            {
                _locationList.Add(partition.Locations[i]);
            }

            _locationList.CollectionChanged += _locationList_CollectionChanged;
            displayList.ItemsSource = _locationList;

            }

        private void _locationList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            displayList.BeginRefresh();
            displayList.EndRefresh();
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

        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            // Add a Observable Collection List that pops down with the paired devices recieved
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