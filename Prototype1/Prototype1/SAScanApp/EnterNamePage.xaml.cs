using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SAScanApp {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterNamePage : ContentPage {

        private ScanPage _scanPage;

        public EnterNamePage() {
            InitializeComponent();
        }

        private void Name_Entered(object sender, EventArgs e) {
            if(NameEntry.Text != string.Empty || NameEntry.Text != null) {
                Continue(); 
            }
        }

        private void Continue_Button_Clicked(object sender, EventArgs e) {
            if(NameEntry.Text != string.Empty || NameEntry.Text != null) {
                Continue(); 
            }
        }

        private async void Continue() {
            if(_scanPage == null) {
                await Navigation.PushAsync(new ScanPage(NameEntry.Text));
            } else {
                await Navigation.PushAsync(_scanPage);
            }
        }

    }
}