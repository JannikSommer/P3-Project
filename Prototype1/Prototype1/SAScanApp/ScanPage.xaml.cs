using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using Model;
using Networking;
using System.Threading.Tasks;

//TODO: FAILSAFE if user returns or quits without uploading.

namespace SAScanApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage {
        private ObservableCollection<LocationBarcode> LocationList { get; set; }
        public string Username { get; set; }
        private RefString ScannedLocation; // Property. Validation.
        private bool lightOn = false;

        public ScanPage(string username) {
            Username = username;
            ScannedLocation = new RefString(string.Empty);
            InitializeComponent();
            LocationList = new ObservableCollection<LocationBarcode>();
            LocationList.CollectionChanged += LocationList_CollectionChanged;
            displayList.ItemsSource = LocationList;
        }


        private void LocationList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            displayList.BeginRefresh();
            displayList.EndRefresh();
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

        private void DisplayList_ItemSelected(object sender, SelectedItemChangedEventArgs e) {
            displayList.SelectedItem = null;
        }

        private async void DisplayList_ItemTapped(object sender, ItemTappedEventArgs e) {
            await Navigation.PushAsync(new LocationSelected((LocationBarcode)e.Item, ScannedLocation));
        }

        private void ButtonV_Clicked(object sender, EventArgs e) {

        }

        public void ScanEditorFocus() {
            if(!ScanEditor.IsFocused) {
                ScanEditor.Focus();
            }
        }

        private async void ScanEditor_TextChanged(object sender, TextChangedEventArgs e) {
            if(ScanEditor.Text != null && ScanEditor.Text != string.Empty && ScanEditor.Text[ScanEditor.Text.Length - 1] == '\n') {
                string barcode = ScanEditor.Text.Substring(0, ScanEditor.Text.Length - 1);
                ScanEditor.Text = string.Empty;
                if(new BarcodeVerifier().IsLocation(barcode)) {
                    LocationBarcode loc = new BarcodeVerifier().GetScannedLocationBarcode(LocationList, barcode);
                    if(loc == null) {
                        loc = new LocationBarcode(barcode, Username);
                    }
                    LocationList.Add(loc);
                    await Navigation.PushAsync(new LocationSelected(loc, ScannedLocation));
                } else {
                    //TODO: Item scanned. Add a popup?
                }
            }
        }

        private async void ContentPage_Appearing(object sender, EventArgs e) {
            if(ScannedLocation.Text != string.Empty) {
                if(new BarcodeVerifier().IsLocation(ScannedLocation.Text)) {
                    LocationBarcode loc = new BarcodeVerifier().GetScannedLocationBarcode(LocationList, ScannedLocation.Text);
                    if(loc == null) {
                        loc = new LocationBarcode(ScannedLocation.Text, Username);
                    }
                    LocationList.Add(loc);
                    await Navigation.PushAsync(new LocationSelected(loc, ScannedLocation));
                }
            } else {
                ScanEditorFocus();
            }
        }


        private async void UploadButtonClicked(object sender, EventArgs e) {
            List<LocationBarcode> data = new List<LocationBarcode>();
            foreach(var item in LocationList) {
                data.Add(item);
            }
            LocationList.Clear();
            await Navigation.PushAsync(new UploadPage(data));
        }

    }
}




