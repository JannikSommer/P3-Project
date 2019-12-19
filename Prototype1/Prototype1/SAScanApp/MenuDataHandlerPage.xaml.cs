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
using System.IO;

namespace SAScanApp {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuDataHandlerPage : ContentPage {
        public MenuDataHandlerPage(string userName) {
            InitializeComponent();
            _userName = userName;
        }
        public MenuDataHandlerPage(string userName, CommunicationFlag partitionType) : this(userName) {
            _type = partitionType;
        }

        private bool _hasPartitionDownloaded = false;
        private string _userName = string.Empty;
        private CommunicationFlag _type = CommunicationFlag.PartitionRequest;

        
        [Obsolete]
        private async void UploadPartition(object sender, EventArgs e) {

        if (!_hasPartitionDownloaded && _type == CommunicationFlag.PartitionUpload)
        {
            Client client = new Client();
            Partition partition = new Model.Partition();
            CommunicationHandler handler = client.UploadPartition(partition);
            if( handler != CommunicationHandler.Success) {
                await DisplayAlert("Error", "You have no active partition", "Okay");
            } else {
                _hasPartitionDownloaded = false;
                await DisplayAlert("Succes", "Partition succesfully uploaded", "Okay");
            }
        } else if(!_hasPartitionDownloaded && _type == CommunicationFlag.VerificationUpload)
            {
                if (_hasPartitionDownloaded != true) {
                    Client client = new Client();
                    VerificationPartition Vpartition = new VerificationPartition();
                    CommunicationHandler handler = client.UploadVerificationPartition(Vpartition);
                    if (handler != CommunicationHandler.Success)
                    {
                        await DisplayAlert("Error", "You have no active Verification Partition", "Okay");

                    } else {
                        _hasPartitionDownloaded = false;
                        await DisplayAlert("Succes", "Verification Partition succesfully uploaded", "Okay");
                    }
                }
            }
        }

        private async void DownloadPartition(object sender, EventArgs e) {
            if (!_hasPartitionDownloaded && _type == CommunicationFlag.PartitionRequest) {
                Client networkingClient = new Client();
                (Partition partition, CommunicationHandler handler) = networkingClient.DownloadPartition(new User(_userName));

                if (handler != CommunicationHandler.Success) {
                    await DisplayAlert("Error", "An error occured", "Ok");
                } else {
                    _hasPartitionDownloaded = true;
                    await Navigation.PushAsync(new ScanPage(partition));
                }
            } else if(!_hasPartitionDownloaded && _type == CommunicationFlag.VerificationRequest) {
                Client networkingClient = new Client();
                (VerificationPartition Vpartition, CommunicationHandler handler) = networkingClient.DownloadVerificationPartition(new User(_userName));

                if (handler != CommunicationHandler.Success) {
                    await DisplayAlert("Error", "An error occured", "Ok");
                } else{
                    _hasPartitionDownloaded = true;
                    await Navigation.PushAsync(new ScanPage(Vpartition));
                }
            }
        }

    }
}