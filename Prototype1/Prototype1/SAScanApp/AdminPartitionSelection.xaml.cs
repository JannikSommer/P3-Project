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
        public AdminPartitionSelection() {
            InitializeComponent();
        }
        public AdminPartitionSelection(AdminLoginPage admPage) : this() {
            _admPage = admPage;
        }
        public AdminPartitionSelection(EnterNamePage eNamePage) : this() {
            _eNamePage = eNamePage;
        }

        private Partition part { get; set; }
        private VerificationPartition Vpart { get; set; }
        private EnterNamePage _eNamePage { get; set; }
        private AdminLoginPage _admPage { get; set; }
        public bool IsVerificationPartition { get; set; }


        private void VerificationPartition_Selected_Button(object sender, EventArgs e) {
            IsVerificationPartition = true;
            new MenuDataHandlerPage(this);
        }

        private void Partition_Selected_Button(object sender, EventArgs e) {
            IsVerificationPartition = false;
            new MenuDataHandlerPage(this);
        }
    }
}