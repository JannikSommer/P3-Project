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
        
        private async void UploadPartition(object sender, EventArgs e)
        {

            Networking.Client networkingClient = new Networking.Client();
            Model.Partition partition = new Model.Partition();
            CommunicationHandler handler = await networkingClient.UploadPartitionAsync(partition);
            if (handler == CommunicationHandler.Success)
            {
                await DisplayAlert("You did it", handler.ToString(), "Incredible");
            }
            else
            {
                await DisplayAlert("You didn't", handler.ToString(), "Fix your shit");
            }
        }

        private async void DownloadPartition(object sender, EventArgs e) // TODO: make async event
        {
            Networking.Client networkingClient = new Networking.Client();
            Central_Controller.Client DeviceClient = new Central_Controller.Client("Anders");
            (Model.Partition partition,  CommunicationHandler handler) = await networkingClient.DownloadPartitionAsync(DeviceClient);
            if (handler != CommunicationHandler.Success)
            {
                await DisplayAlert("Error", "An error occured", "Fix your shit!");
            }
            else
            {
                await DisplayAlert(handler.ToString(), partition.Locations[0].ID , "You fixed your shit!");
            }
        }
    }
}