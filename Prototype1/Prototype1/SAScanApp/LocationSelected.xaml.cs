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
        public LocationSelected() {
            InitializeComponent();
            quantity.Text = Convert.ToString(Value);
            _counterEnabled = false;
        }
        public LocationSelected(List<Item> itemList, Partition partition) : this() {
            _itemList = new ObservableCollection<Item>(itemList);
            _itemList.CollectionChanged += _itemList_CollectionChanged;
            itemDisplayList.ItemsSource = _itemList;
            _partition = partition;
        }

        public int Value {
            get { return _value; }
            set {
                _value = value;
                quantity.Text = Convert.ToString(Value);
            }
        }
        public Partition _partition { get; set; }

        private ObservableCollection<Item> _itemList;
        private Item _prevItem;
        private bool _counterEnabled;
        private bool lightOn = false;
        private int _value;


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

        private async void Menu_Button_Clicked(object sender, EventArgs e) {
            
        }

        private void ItemDisplayList_ItemTapped(object sender, ItemTappedEventArgs e) {
            if(_prevItem == ((Item)e.Item)) {
                // Deselect item
                itemDisplayList.SelectedItem = null;
                _counterEnabled = false;
                inc_item_count.IsEnabled = false;
                dec_item_count.IsEnabled = false;
                _prevItem = null;
            } else {
                ChangeSelectedItem((Item)e.Item);
            }
            ScanEditorFocus();
        }

        private void ChangeSelectedItem(Item item) {
            SaveQuantity(_prevItem);
            _prevItem = item;
            itemDisplayList.SelectedItem = item;
            Value = ((Item)itemDisplayList.SelectedItem).CountedQuantity;
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

        private void SaveQuantity(Item item) {
            if(item != null) {
                item.CountedQuantity = Value;
            }
        }

        private void ScanEditor_TextChanged(object sender, TextChangedEventArgs e) {
            if(ScanEditor.Text != null && ScanEditor.Text != string.Empty && ScanEditor.Text[ScanEditor.Text.Length - 1] == '\n') {
                string barcode = ScanEditor.Text.Substring(0, ScanEditor.Text.Length - 1);
                ScanEditor.Text = string.Empty;
                UpdateItem(barcode);
                ScanEditorFocus();
            }
        }

        private void UpdateItem(string barcode) {
            Item item = new BarcodeVerifier().GetScannedItem(_partition, barcode);
            if(item != null) {
                ChangeSelectedItem(item);
                IncrementValue();
            }  
        }
    }
}