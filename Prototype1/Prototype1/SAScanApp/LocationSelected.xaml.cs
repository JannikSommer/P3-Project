using SAScanApp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Model;


namespace SAScanApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationSelected : ContentPage
    {
        public int Value {
            get { return _value; }
            set {
                _value = value;
                quantity.Text = Convert.ToString(Value);
            }
        }

        private ScanPage _scanPage { get; set; }
        private ObservableCollection<Item> _itemList;
        private List<Item> _returnItems;
        private Item _prevItem;
        private bool _counterEnabled;
        private bool lightOn = false;
        private int _value { get; set; }
        
        public LocationSelected()
        {
            InitializeComponent();
            quantity.Text = Convert.ToString(Value);
            _counterEnabled = false;
        }

        public LocationSelected(ScanPage startPage) : this()
        {
            _scanPage = startPage;
        }

        public LocationSelected(ScanPage startPage, List<Item> itemList) : this(startPage)
        {
            _returnItems = itemList;
            _itemList =  new ObservableCollection<Item>(itemList);
            _itemList.CollectionChanged += _itemList_CollectionChanged;
            itemDisplayList.ItemsSource = _itemList;
        }

        private void _itemList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            itemDisplayList.BeginRefresh();
            itemDisplayList.EndRefresh();
        }

        private void dec_item_count_Clicked(object sender, EventArgs e)
        {
            if (_counterEnabled == true && Value > 0)
            {
                Value--;
            }
        }

        private void inc_item_count_Clicked(object sender, EventArgs e)
        {
            if (_counterEnabled == true)
            {
                Value++;
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
            DisplayAlert("Test", "Test", "Test");
        }

        private async void Menu_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuStartPage(this));
        }

        private void itemDisplayList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if(_prevItem == ((Item)e.Item)) {
                // Deselect item
                itemDisplayList.SelectedItem = null;
                _counterEnabled = false;
                inc_item_count.IsEnabled = false;
                dec_item_count.IsEnabled = false;
                _prevItem = null;
            } else {
                // Save previous quantity
                SaveQuantity((Item)_prevItem);

                _prevItem = (Item)e.Item;
                Value = ((Item)itemDisplayList.SelectedItem).CountedQuantity;
            }
        }

        private void itemDisplayList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(_counterEnabled != true) {
                _counterEnabled = true;
                inc_item_count.IsEnabled = true;
                dec_item_count.IsEnabled = true;
            }

        }

        private void ContentPage_Disappearing(object sender, EventArgs e) {
            SaveQuantity((Item)itemDisplayList.SelectedItem);

            for(int i = 0; i < _returnItems.Count; i++) {
                _returnItems[i].CountedQuantity = _itemList[i].CountedQuantity;
            }
        }

        private void SaveQuantity(Item item) {
            if(item != null) {
                item.CountedQuantity = Value;
            }
        }
    }
}