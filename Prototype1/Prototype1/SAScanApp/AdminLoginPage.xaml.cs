using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SAScanApp {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminLoginPage : ContentPage {

        string _fileName;
        
        public AdminLoginPage(string fileName) {
            InitializeComponent();
            _fileName = fileName;
        }

        public AdminLoginPage(string fileName, string userName)
        {
            InitializeComponent();
            _fileName = fileName;
            userLogin.Text = userName;
        }

        public async void Entry_Completed(object sender, EventArgs e) {
            if(userLogin.Text == "" || userLogin.Text == " " || userLogin.Text == "text" || userLogin.Text == "Text")
            {
                await DisplayAlert("Error!", "Please enter your username", "Ok");
            }
            else if (userPassword.Text == "password") {
                File.WriteAllText(_fileName, "*ADMIN " + userLogin.Text);
                await Navigation.PushAsync(new AdminPartitionSelection(userLogin.Text));
            } else {
                await DisplayAlert("Error!", "Wrong password, please try again", "Ok");
            }
        }    
    }
}