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

            // Der skal addes noget typesafety her, så hvis eventet fyrer igen, imens man er igang med at uploade en partition, så sker der ikke noget
            // Ligeledes er ens partition ikke done, (mangler en location/item som slet ikke er scannet) så kan den ikke uploades (måske med overrule funktion??)
            
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

            // Typesafety, samme som ovenstående, plus at hvis ens partition pt. ikke er uploaded, kan man ikke få en ny
           
        }

        
    }
}