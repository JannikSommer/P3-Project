using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Networking;

namespace SAScanApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuDataHandlerPage : ContentPage
    {

        private MenuStartPage _mStartPage;
        public MenuDataHandlerPage(MenuStartPage mStartPage) { 


        InitializeComponent();
        _mStartPage = mStartPage;

        }
        
        private void UploadPartition(object sender, EventArgs e)
        {
            Client client = new Client();
            Model.Partition partition = new Model.Partition();
            CommunicationHandler handler = client.UploadPartitionAsync(partition).Result;

            DependencyService.Get<IBluetoothHandler>().closeBluetoothConnection();
        }

        private void DownloadPartition(object sender, EventArgs e) // TODO: make async event
        {
            Client networkingClient = new Client();
            Central_Controller.Client DeviceClient = new Central_Controller.Client("Anders");
            (Model.Partition partition,  CommunicationHandler handler) = networkingClient.DownloadPartition(DeviceClient);
            if (handler != CommunicationHandler.Success)
            {
                DisplayAlert("Error", "An error occured", "Fix your shit!");
            }
            else
            {
                DisplayAlert(handler.ToString(), partition.Locations[0].ID , "You fixed your shit!");
            }

            DependencyService.Get<IBluetoothHandler>().enableBluetooth();
        }
    }
}