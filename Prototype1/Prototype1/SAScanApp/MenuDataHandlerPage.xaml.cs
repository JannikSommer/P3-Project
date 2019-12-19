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
        private MainPage _mPage { get; set; }
        private AdminPartitionSelection _admPSPage { get; set; }
        private MenuStartPage _mStartPage { get; set; }
        public bool IsPartitionDownloaded { get; set; } = false;
        public string UserName { get; set; }


        public MenuDataHandlerPage()
        {
            InitializeComponent();
        }

        public MenuDataHandlerPage(MainPage mPage)
            : this() 
        {
            _mPage = mPage;
        }

        public MenuDataHandlerPage(AdminPartitionSelection admPSPage)
            : this()
        {
            _admPSPage = admPSPage;
        }

        public MenuDataHandlerPage(MenuStartPage mStartPage) 
            : this()
        {
            _mStartPage = mStartPage;
        }
        
        
        private async void UploadPartition(object sender, EventArgs e) {

        if (!IsPartitionDownloaded && !_admPSPage.IsVerificationPartition)
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

        else if(!IsPartitionDownloaded && _admPSPage.IsVerificationPartition == true)
            {
                if (IsPartitionDownloaded != true)
                {
                    Client client = new Client();
                    VerificationPartition Vpartition = new Model.VerificationPartition();
                    CommunicationHandler handler = client.UploadVerificationPartition(Vpartition);
                    if (handler != CommunicationHandler.Success)
                    {
                        await DisplayAlert("Error", "You have no active Verification Partition", "Okay");

                    }

                    else
                    {
                        IsPartitionDownloaded = false;
                        await DisplayAlert("Succes", "Verification Partition succesfully uploaded", "Okay");
                    }
                }
            }



            // Der skal addes noget typesafety her, så hvis eventet fyrer igen, imens man er igang med at uploade en partition, så sker der ikke noget
            // Ligeledes er ens partition ikke done, (mangler en location/item som slet ikke er scannet) så kan den ikke uploades (måske med overrule funktion??)
        }

        private async void DownloadPartition(object sender, EventArgs e) 
        {

            if (IsPartitionDownloaded && !_admPSPage.IsVerificationPartition)
            {
                var path = Environment.CurrentDirectory + @"\UserData\UserName.txt";

                var UserName = File.ReadAllText(path);                

                Client networkingClient = new Client();
                Central_Controller.Client DeviceClient = new Central_Controller.Client(UserName);
                (Partition partition, CommunicationHandler handler) = networkingClient.DownloadPartition(DeviceClient);

                if (handler != CommunicationHandler.Success)
                {
                    await DisplayAlert("Error", "An error occured", "Fix your shit!");
                }
                else
                {
                    IsPartitionDownloaded = true;
                    await Navigation.PushAsync(new ScanPage(partition));
                }
            }

            else if(IsPartitionDownloaded && _admPSPage.IsVerificationPartition)
            {
                Client networkingClient = new Client();
                Central_Controller.Client DeviceClient = new Central_Controller.Client(UserName);
                (VerificationPartition Vpartition, CommunicationHandler handler) = networkingClient.DownloadVerificationPartition(DeviceClient);

                if (handler != CommunicationHandler.Success)
                {
                    await DisplayAlert("Error", "An error occured", "Fix your shit!");
                }
                else
                {
                    IsPartitionDownloaded = true;
                    await Navigation.PushAsync(new ScanPage(Vpartition));
                }
            }
        }

    }
}