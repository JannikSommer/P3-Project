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

        private bool _hasPartitionDownloaded = false;
        private string _userName = string.Empty;
        private CommunicationFlag _type = CommunicationFlag.PartitionRequest;
        private App _appPage { get; set; }
        private EnterNamePage _enterNamePage { get; set; }

        public MenuDataHandlerPage()
        {
            InitializeComponent();

            if (_hasPartitionDownloaded == true)
            {
                UploadButton.IsVisible = true;
            }

            else
            {
                DownloadButton.IsVisible = true;
            }
        }

        public MenuDataHandlerPage(string userName, App appPage) : this() {
            _appPage = appPage;
            _userName = userName;
        }

        public MenuDataHandlerPage(string userName, EnterNamePage namePage) : this ()
        {
            _enterNamePage = namePage;
            _userName = userName;
        }
        public MenuDataHandlerPage(string userName, CommunicationFlag partitionType) : this() {
            _type = partitionType;
        }              
       
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

        private async void CreateTestPartition_Clicked(object sender, EventArgs e)
        {
           
            if (_type == CommunicationFlag.PartitionRequest)
            {
                await Navigation.PushAsync(new ScanPage(
                    new Partition(
                        new Model.Location("000A01",
                        new List<Item> {
                        new Item("5701872203005"),
                        new Item("73102601"),
                        new Item ("8979878"),
                        new Item ("78789"),
                        new Item ("5709216007104")
                        })
                    )));
            }
            else
            {
                await Navigation.PushAsync(new ScanPage(new VerificationPartition()
                {
                    Locations = new List<Model.Location> {
                    new Model.Location("000A01",
                    new List<Item> {
                        new Item("5701872203005"),
                        new Item("73102601"),
                        new Item ("8979878"),
                        new Item ("78789"),
                        new Item ("5709216007104")
                    }
                    )}
                }));
            }
            
        }
    }
}