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

        public EnterNamePage() {
            InitializeComponent();
        }

        private async void Name_Entered(object sender, EventArgs e) {
            await Navigation.PushAsync(new ScanPage(NameEntry.Text));
        }

        private async void Continue_Button_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new ScanPage(NameEntry.Text));
        }
    }
}