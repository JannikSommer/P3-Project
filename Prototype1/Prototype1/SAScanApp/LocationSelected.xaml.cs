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
    public partial class LocationSelected : ContentPage
    {
        private ScanPage _scanPage { get; set; }
        private List<Model.Item> _itemList { get; set; }

        public bool _counterEnabled { get; set; }
        bool lightOn = false;
        uint value = 00;

        public LocationSelected()
        {
            InitializeComponent();
            quantity.Text = Convert.ToString(value);
            _counterEnabled = false;
        }
        public LocationSelected(ScanPage startPage)
            : this()
        {
            _scanPage = startPage;
        }

        public LocationSelected(ScanPage startPage, List<Model.Item> itemList)
            : this()
        {
            _scanPage = startPage;
            _itemList = itemList;
            itemDisplayList.ItemsSource = _itemList;
        }

        private void dec_item_count_Clicked(object sender, EventArgs e)
        {
            if (_counterEnabled == true && value > 0)
            {
                quantity.Text = Convert.ToString(--value);
            }
        }

        private void inc_item_count_Clicked(object sender, EventArgs e)
        {
            if (_counterEnabled == true)
            {
                quantity.Text = Convert.ToString(++value);
            }
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

        private async void Menu_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuStartPage(this));
        }

        private void itemDisplayList_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void itemDisplayList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _counterEnabled = true;
        }
    }
}