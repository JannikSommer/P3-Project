using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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
        }

        private void DownloadPartition(object sender, EventArgs e)
        {
            Client networkingClient = new Client();
            Central_Controller.Client DeviceClient = new Central_Controller.Client("Anders");
            (Model.Partition partition,  CommunicationHandler handler) = networkingClient.DownloadPartitionAsync(DeviceClient).Result;
        }
    }
}