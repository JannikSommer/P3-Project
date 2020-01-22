using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking;
using Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SAScanApp {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadPage : ContentPage {
        private List<LocationBarcode> _data;

        public UploadPage(List<LocationBarcode> data) {
            InitializeComponent();
            _data = data;
        }

        private async void ContentPage_Appearing(object sender, EventArgs e) {
            Client netClient = new Client();
            CommunicationHandler handler = netClient.UploadStatus(_data);
            if(handler != CommunicationHandler.Success) {
                await DisplayAlert("Error", "Something went wrong.", "Okay");
            } else {
                await DisplayAlert("Succes", "Data succesfully uplaoded.", "Okay");
            }
            await Navigation.PopAsync();
        }
    }
}