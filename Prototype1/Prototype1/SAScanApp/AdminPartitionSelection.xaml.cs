using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Model;
using Networking;

namespace SAScanApp {
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class AdminPartitionSelection : ContentPage {
        public AdminPartitionSelection(string userName) {
            InitializeComponent();
            _userName = userName;
        }

        private string _userName;
        private Partition _partition { get; set; }
        private VerificationPartition _verificationPartition { get; set; }

        public bool IsVerificationPartition { get; set; }


        private async void VerificationPartition_Selected_Button(object sender, EventArgs e) {
            IsVerificationPartition = true;
            await Navigation.PushAsync(new MenuDataHandlerPage(_userName, CommunicationFlag.VerificationRequest));
        }

        private async void Partition_Selected_Button(object sender, EventArgs e) {
            IsVerificationPartition = false;
            await Navigation.PushAsync(new MenuDataHandlerPage(_userName, CommunicationFlag.PartitionRequest));
        }
    }
}