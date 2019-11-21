using SAScanApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace SAScanApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BS_LocationSelected : ContentPage
    {
        private BS_StartPage _bsStartPage { get; set; }
        private List<Item> _itemList { get; set; }

        public bool _counterEnabled { get; set; }
        bool lightOn = false;
        uint value = 00;

        public BS_LocationSelected()
        {
            InitializeComponent();
            quantity.Text = Convert.ToString(value);
            _counterEnabled = false;
        }
        public BS_LocationSelected(BS_StartPage startPage)
            : this()
        {
            _bsStartPage = startPage;           
        }

        public BS_LocationSelected(BS_StartPage startPage, List<Item> itemList)
            : this()
        {
            _bsStartPage = startPage;
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
            await Navigation.PushAsync(new M_StartPage(this));
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