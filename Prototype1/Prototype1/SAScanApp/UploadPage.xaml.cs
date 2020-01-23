using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking;
using Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace SAScanApp {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadPage : ContentPage {
        private List<LocationBarcode> _data;
        private ObservableCollection<LocationBarcode> _realdata;

        public UploadPage(ObservableCollection<LocationBarcode> data) {
            InitializeComponent();
            _realdata = data;
            _data = new List<LocationBarcode>();
            foreach(var item in data) {
                _data.Add(item);
            }
        }

        private async void ContentPage_Appearing(object sender, EventArgs e) {
            if(_data.Count > 0) {
                try {
                    Client netClient = new Client();
                    CommunicationHandler handler = netClient.UploadStatus(_data);
                    if(handler != CommunicationHandler.Success) {
                        await DisplayAlert("Error", "Something went wrong.", "Okay");
                    } else {
                        _realdata.Clear();
                        await DisplayAlert("Succes", "Data succesfully uplaoded.", "Okay");
                    }
                } catch {
                    await DisplayAlert("Error", "An error occurred, please try again.", "Okay");
                } 
            } else {
                await DisplayAlert("No data", "No data to upload.", "Okay");
            }
            await Navigation.PopAsync();
        }
    }
}