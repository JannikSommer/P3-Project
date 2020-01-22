using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Model;


namespace SAScanApp {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationSelected : ContentPage {
        private LocationSelected() {
            InitializeComponent();
            quantity.Text = Convert.ToString(Value);
            _counterEnabled = false;
            ScanEditorFocus();
            }

            public LocationSelected(LocationBarcode locationBarcode, RefString scannedLocation) : this() {
            scannedLocation.Text = string.Empty;
            _locationBarcode = locationBarcode;
            _scannedLocation= scannedLocation;
            _itemList = new ObservableCollection<ItemBarcode>(_locationBarcode.ItemBarcodes);
            _itemList.CollectionChanged += _itemList_CollectionChanged;
            itemDisplayList.ItemsSource = _itemList;
        }

        public int Value {
            get { return _value; }
            set {
                _value = value;
                quantity.Text = Convert.ToString(Value);
            }
        }

        private RefString _scannedLocation;
        private LocationBarcode _locationBarcode;
        private ObservableCollection<ItemBarcode> _itemList;
        private ItemBarcode _prevItem;
        private bool _counterEnabled;
        private bool lightOn = false;
        private int _value = 0;


        public void ScanEditorFocus() {
            if(!ScanEditor.IsFocused) {
                ScanEditor.Focus();
            }
        }

        public void DecrementValue() {
            if (_counterEnabled == true && Value > 0) {
                Value--;
            }
        }

        public void IncrementValue() {
            if (_counterEnabled == true) {
                Value++;
            }
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            ScanEditorFocus();
        }

        private void _itemList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            itemDisplayList.BeginRefresh();
            itemDisplayList.EndRefresh();
        }

        private void dec_item_count_Clicked(object sender, EventArgs e) {
            DecrementValue();
            ScanEditorFocus();
        }

        private void inc_item_count_Clicked(object sender, EventArgs e) {
            IncrementValue();
            ScanEditorFocus();
        }

        private async void Light_Button_Clicked(object sender, EventArgs e) {
            try {
                if (lightOn == false) {
                    // Turn On Flashlight  
                    await Flashlight.TurnOnAsync();
                    lightOn = true;
                } else if (lightOn == true) {
                    await Flashlight.TurnOffAsync();
                    lightOn = false;
                }
            }

            catch (FeatureNotSupportedException) {
                await DisplayAlert("Error:", "Your device does not support the use of this feature not supported, sorry!", "Okay");
            }
            catch (PermissionException) {
                await DisplayAlert("Error:", "Enable permissions to access flashlight in your phone", "Okay");
            }
            catch (Exception) {
                await DisplayAlert("Error", "There has been an error", "Okay");
            }


        }


        private void ItemDisplayList_ItemTapped(object sender, ItemTappedEventArgs e) {
            if(_prevItem == ((ItemBarcode)e.Item)) {
                // Deselect item
                itemDisplayList.SelectedItem = null;
                _counterEnabled = false;
                inc_item_count.IsEnabled = false;
                dec_item_count.IsEnabled = false;
                _prevItem = null;
            } else {
                ChangeSelectedItem((ItemBarcode)e.Item);
            }
            ScanEditorFocus();
        }

        private void ChangeSelectedItem(ItemBarcode item) {
            SaveQuantity(_prevItem);
            _prevItem = item;
            itemDisplayList.SelectedItem = item;
            Value = ((ItemBarcode)itemDisplayList.SelectedItem).Quantity;
            ScanEditorFocus();
        }

        private void ItemDisplayList_ItemSelected(object sender, SelectedItemChangedEventArgs e) {
            if(_counterEnabled != true) {
                _counterEnabled = true;
                inc_item_count.IsEnabled = true;
                dec_item_count.IsEnabled = true;
            }
            ScanEditorFocus();
        }

        private void SaveQuantity(ItemBarcode item) {
            if(item != null) {
                item.Quantity = Value;
            }
        }

        private void ScanEditor_TextChanged(object sender, TextChangedEventArgs e) {
            if(ScanEditor.Text != null && ScanEditor.Text != string.Empty && ScanEditor.Text[ScanEditor.Text.Length - 1] == '\n') {
                string barcode = ScanEditor.Text.Substring(0, ScanEditor.Text.Length - 1);
                ScanEditor.Text = string.Empty;
                if(new BarcodeVerifier().IsLocation(barcode)) {
                    _scannedLocation.Text = barcode;
                    Navigation.PopAsync();
                } else {
                    UpdateItem(barcode);
                    ScanEditorFocus();
                }
            }
        }

        private void UpdateItem(string barcode) {
            ItemBarcode item = new BarcodeVerifier().GetScannedItemBarcode(_itemList, barcode);
            if(item == null) { // Item is not in list yet. Add it.
                item = new ItemBarcode(barcode);
            }
            _itemList.Add(item);

            ChangeSelectedItem(item);
            IncrementValue();
        }

        private void ContentPage_Disappearing(object sender, EventArgs e) {
            SaveQuantity(_prevItem);
            List<ItemBarcode> result = new List<ItemBarcode>();
            foreach(var item in _itemList) {
                result.Add(item);
            }
            _locationBarcode.ItemBarcodes = result;
        }

    }
}