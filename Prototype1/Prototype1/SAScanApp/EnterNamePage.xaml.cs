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
            if(NameEntry.Text != string.Empty && NameEntry.Text != null && Char.IsLetterOrDigit(NameEntry.Text.First())) {
                Continue(); 
            }
        }

        private void Continue_Button_Clicked(object sender, EventArgs e) {
            if(NameEntry.Text != string.Empty && NameEntry.Text != null && Char.IsLetterOrDigit(NameEntry.Text.First())) {
                Continue(); 
            }
        }

        private async void Continue() {
            if(_scanPage == null || _scanPage.Username != NameEntry.Text) {
                _scanPage = new ScanPage(NameEntry.Text);
            } 
                await Navigation.PushAsync(_scanPage);
        }

    }
}