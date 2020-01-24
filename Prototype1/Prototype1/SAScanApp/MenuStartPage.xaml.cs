using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking;
using Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace SAScanApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuStartPage : ContentPage {
        public MenuStartPage(Partition partition) {
            InitializeComponent();
            _partition = partition;
        }
        public MenuStartPage(VerificationPartition verificationPartition) {
            InitializeComponent();
            _verificationPartition = verificationPartition;
        }

        private bool _hasPartitionDownloaded = false;
        private Partition _partition;
        private VerificationPartition _verificationPartition;


        private async void AdminLogin_Button_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new AdminLoginPage(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "username.txt")));
        }

        private async void GetData_Button_Clicked(object sender, EventArgs e) {
            if(!_hasPartitionDownloaded && _partition != null) {
                Client client = new Client();
                CommunicationHandler handler = client.UploadPartition(_partition);
                if(handler != CommunicationHandler.Success) {
                    await DisplayAlert("Error", "You have no active partition", "Okay");
                } else {
                    _hasPartitionDownloaded = false;
                    await DisplayAlert("Succes", "Partition succesfully uploaded", "Okay");
                }
            } else if(!_hasPartitionDownloaded && _verificationPartition != null) {
                if(_hasPartitionDownloaded != true) {
                    Client client = new Client();
                    CommunicationHandler handler = client.UploadVerificationPartition(_verificationPartition);
                    if(handler != CommunicationHandler.Success) {
                        await DisplayAlert("Error", "You have no active Verification Partition", "Okay");
                    } else {
                        _hasPartitionDownloaded = false;
                        await DisplayAlert("Succes", "Verification Partition succesfully uploaded", "Okay");
                    }
                }
            }

            await Navigation.PopModalAsync();
        }

        private void Terminate_Button_Clicked(object sender, EventArgs e) {

        }


    }
}