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
        private string _fileName { get; set; }
        private App _appPage { get; set; }

        public EnterNamePage() {
            InitializeComponent();
        }

        public EnterNamePage(App appPage) : this() {
            _appPage = appPage;
        }

        private async void Save_Name_Clicked(object sender, EventArgs e) {
            _userName = NameEntry.Text;            
            _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "username.txt");
            File.WriteAllText(_fileName, _userName);
            await Navigation.PushAsync(new MenuDataHandlerPage(_userName, this));
        }

        private async void AdminLogin_Button_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new AdminLoginPage());
        }
    }
}