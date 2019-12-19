using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SAScanApp {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminLoginPage : ContentPage {
        public AdminLoginPage() {
            InitializeComponent();
        }

        public async void Entry_Completed(object sender, EventArgs e) {
            var usrNme = userLogin.Text;
            var pssWrd = userPassword.Text;
           
            if (usrNme == "admin" && pssWrd == "password") {
                await Navigation.PushAsync(new AdminPartitionSelection(usrNme));
            } else {
                await DisplayAlert("Error!", "Wrong password, please try again", "Ok");
            }
        }    
    }
}