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
        public Partition partition { get; set; }
        public List<Model.Location> _locationList { get; set; }
        public List<Item> _itemList { get; set; }
        
        bool lightOn = false;
        
        public BS_StartPage()
        {     
            InitializeComponent();
            _itemList = new List<Item> { new Item {
                ItemID = "10",
                Name = "Adidas Hoodie",
                Color = "Blå",
                Size = "XL",
                Checksum = "72312390",
                ImageUrl = "http://lorempixels.com/100/100/1.jpg",
                HasMultiLocation = false,
                },

            new Item {
                ItemID = "20",
                Name = "Adidas Shoe",
                Color = "Grå",
                Size = "42",
                Checksum = "72839281",
                ImageUrl = "http",
                HasMultiLocation = false
                },


            new Item {
                ItemID = "30",
                Name = "Adidas T-Shirt",
                Color = "Orange",
                Size = "S",
                Checksum = "72223390",
                ImageUrl = "http://lorempixels.com/100/100/1.jpg",
                HasMultiLocation = false}
            };

            _locationList = new List<Model.Location> { new Model.Location { 
                LocationID = "1", 
                IsEmpty = false,
                Shelf = 1, 
                Row = "1", 
                Posistion = "noget", 
                Items = _itemList},

                new Model.Location {
                LocationID = "2",
                IsEmpty = false,
                Shelf = 1,
                Row = "1",
                Posistion = "noget",
                Items = _itemList},

                new Model.Location {
                LocationID = "3",
                IsEmpty = false,
                Shelf = 1,
                Row = "1",
                Posistion = "noget",
                Items = _itemList}
            };

            partition = new Partition()
            {
                State = PartitionState.NotCounted,
                TotalNrOFItems = _locationList.Count,
                ItemsCounted = 1, /* Skriv en funktion til at tælle dem */
                RequsitionState = PartitionRequsitionState.Requested,
                Locations = _locationList
            };       
            displayList.ItemsSource = _locationList;
        }

        public BS_StartPage(Partition partition)
            :this()
        {

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

        private async void displayList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
            displayList.SelectedItem = null;
        }

        private async void displayList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
             await Navigation.PushAsync(new BS_LocationSelected(this, _itemList));
        }
    }
}