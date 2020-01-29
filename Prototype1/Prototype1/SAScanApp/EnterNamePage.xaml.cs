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
        private string _userName { get; set; }
        private string _fileName { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "username.txt");
        private App _appPage { get; set; }

        public EnterNamePage() {
            InitializeComponent();
            if (File.Exists(_fileName) && System.IO.File.ReadAllText(_fileName) != "")
            {
                if (System.IO.File.ReadAllText(_fileName).Length > 7 && System.IO.File.ReadAllText(_fileName).Substring(0,7) == "*ADMIN ")
                {
                    _userName = System.IO.File.ReadAllText(_fileName).Substring(7);
                    NameEntry.Text = _userName;
                    LoadNextPage(true);
                }
                else if(System.IO.File.ReadAllText(_fileName) != "*ADMIN ")
                {
                    _userName = System.IO.File.ReadAllText(_fileName);
                    NameEntry.Text = _userName;
                    LoadNextPage(false);
                }
            }
        }

        public EnterNamePage(App appPage) : this() {
            _appPage = appPage;
        }

        private async void LoadNextPage(bool IsAdmin)
        {
            if (IsAdmin)
            {
                await Navigation.PushAsync(new AdminPartitionSelection(_userName));
            }
            else
            {
                await Navigation.PushAsync(new MenuDataHandlerPage(_userName, this));
            }
        }

        private async void Save_Name_Clicked(object sender, EventArgs e) {
            if(NameEntry.Text == "Please enter your name" || NameEntry.Text == "")
            {
                await DisplayAlert("Error!", "Please enter a your name", "Ok");
            }
            else
            {
                _userName = NameEntry.Text;
                File.WriteAllText(_fileName, _userName);
                await Navigation.PushAsync(new MenuDataHandlerPage(_userName, this));
            }
        }

        private async void AdminLogin_Button_Clicked(object sender, EventArgs e) {
            if(NameEntry.Text != "")
            {
                await Navigation.PushAsync(new AdminLoginPage(_fileName, _userName));
            }
            else
            {
                await Navigation.PushAsync(new AdminLoginPage(_fileName));
            }
        }

        private void NameEntry_Focused(object sender, FocusEventArgs e)
        {
            if(NameEntry.Text == "Please enter your name")
            {
                NameEntry.Text = "";
            }
        }

        private void NameEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if(NameEntry.Text == "")
            {
                NameEntry.Text = "Please enter your name";
            }
        }
    }
}