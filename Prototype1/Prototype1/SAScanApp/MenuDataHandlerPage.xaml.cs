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

        public bool IsPartitionDownloaded { get; set; } = false;

        public MenuDataHandlerPage()
        {
            InitializeComponent();
        }

        public MenuDataHandlerPage(MainPage mPage):this() { 
        
        _mPage = mPage;
        }

        public MenuDataHandlerPage(MenuStartPage mStartPage):this()
        {
            _mStartPage = mStartPage;
        }
        private async void UploadPartition(object sender, EventArgs e) 
        {
            Client client = new Client();
            List<Model.Location> locations = new List<Model.Location>();
            locations.Add(new Model.Location("001A02"));
            locations.Add(new Model.Location("001B02"));
            locations.Add(new Model.Location("001C02"));
            locations.Add(new Model.Location("001D02"));

            locations[0].Items.Add(new Item() { UpcBarcode = "474174", EanBarcode = "" });
            locations[1].Items.Add(new Item() { UpcBarcode = "571425", EanBarcode = "" });
            locations[2].Items.Add(new Item() { UpcBarcode = "637413", EanBarcode = "" });

            client.UploadStatus(locations);
            //if (IsPartitionDownloaded != true)
            //{
            //    Client client = new Client();
            //    Partition partition = new Model.Partition();
            //    CommunicationHandler handler = client.UploadPartition(partition);
            //    if( handler != CommunicationHandler.Success)
            //    {
            //        await DisplayAlert("Error", "You have no active partition", "Okay");

            //    }

            //    else
            //    {
            //        IsPartitionDownloaded = false;
            //        await DisplayAlert("Succes", "Partition succesfully uploaded", "Okay");
            //    }
            //}
            // Der skal addes noget typesafety her, så hvis eventet fyrer igen, imens man er igang med at uploade en partition, så sker der ikke noget
            // Ligeledes er ens partition ikke done, (mangler en location/item som slet ikke er scannet) så kan den ikke uploades (måske med overrule funktion??)
        }

        private void DownloadPartition(object sender, EventArgs e) 
        {
            Client networkingClient = new Client();
            Central_Controller.Client DeviceClient = new Central_Controller.Client("Anders");
            (Partition partition,  CommunicationHandler handler) = networkingClient.DownloadPartition(DeviceClient);

            if (handler != CommunicationHandler.Success) {
                DisplayAlert("Error", "An error occured", "Fix your shit!");
            } else {

                IsPartitionDownloaded = true;
                Navigation.PushAsync(new ScanPage(partition));
            }


            // DependencyService.Get<IBluetoothHandler>().EnableBluetooth();
            // Typesafety, samme som ovenstående, plus at hvis ens partition pt. ikke er uploaded, kan man ikke få en ny
        }


    }
}