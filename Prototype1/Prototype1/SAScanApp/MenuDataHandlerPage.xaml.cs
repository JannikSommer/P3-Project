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
using Central_Controller.Central_Controller;


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
            private async void UploadPartition(object sender, EventArgs e) {

            if (IsPartitionDownloaded != true)
            {
                Client client = new Client();
                Partition partition = new Model.Partition();
                CommunicationHandler handler = client.UploadPartition(partition);
                if( handler != CommunicationHandler.Success)
                {
                    await DisplayAlert("Error", "You have no active partition", "Okay");

                }

                else
                {
                    IsPartitionDownloaded = false;
                    await DisplayAlert("Succes", "Partition succesfully uploaded", "Okay");
                }
            }



            // Der skal addes noget typesafety her, så hvis eventet fyrer igen, imens man er igang med at uploade en partition, så sker der ikke noget
            // Ligeledes er ens partition ikke done, (mangler en location/item som slet ikke er scannet) så kan den ikke uploades (måske med overrule funktion??)
        }

        private void DownloadPartition(object sender, EventArgs e) 
        {
            Client networkingClient = new Client();
            User DeviceUser = new User("Anders");
            (Partition partition,  CommunicationHandler handler) = networkingClient.DownloadPartition(DeviceUser);

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