using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Networking;
using Model;

namespace SAScanApp {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuDataHandlerPage : ContentPage {
        private MainPage _mPage { get; set; }
        private MenuStartPage _mStartPage { get; set; }

        private bool IsPartitionDownloaded { get; set; } = false;

        public MenuDataHandlerPage(MainPage mPage) { 
        InitializeComponent();
        _mPage = mPage;
        }

        public MenuDataHandlerPage(MenuStartPage mStartPage)
        {
            _mStartPage = mStartPage;
        }
            private void UploadPartition(object sender, EventArgs e) {

            if (IsPartitionDownloaded != true)
            {
                Client client = new Client();
                Partition partition = new Model.Partition();
                CommunicationHandler handler = client.UploadPartitionAsync(partition).Result;
                if( handler == CommunicationHandler.Success)
                {
                    IsPartitionDownloaded = false;
                }

                else
                {
                    DisplayAlert("Error", "You have no active partition", "Okay");
                }
            }



            // Der skal addes noget typesafety her, så hvis eventet fyrer igen, imens man er igang med at uploade en partition, så sker der ikke noget
            // Ligeledes er ens partition ikke done, (mangler en location/item som slet ikke er scannet) så kan den ikke uploades (måske med overrule funktion??)
        }

        private async void DownloadPartition(object sender, EventArgs e) 
        {
            Client networkingClient = new Client();
            Central_Controller.Client DeviceClient = new Central_Controller.Client("Anders");
            (Partition partition,  CommunicationHandler handler) = await networkingClient.DownloadPartitionAsync(DeviceClient);

            if (handler != CommunicationHandler.Success) {
                await DisplayAlert("Error", "An error occured", "Fix your shit!");
            } else {

                IsPartitionDownloaded = true;
                await Navigation.PushAsync(new ScanPage(partition));
            }


            // DependencyService.Get<IBluetoothHandler>().EnableBluetooth();
            // Typesafety, samme som ovenstående, plus at hvis ens partition pt. ikke er uploaded, kan man ikke få en ny
        }


    }
}