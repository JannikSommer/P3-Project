using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SAScanApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuStartPage : ContentPage
    {
        private ScanPage _scanPage { get; set; }
        private LocationSelected _locSelPage { get; set; }
        public MenuStartPage(ScanPage scanPage)
        {
            InitializeComponent();
            _scanPage = scanPage;
        }

        public MenuStartPage(LocationSelected locSelPage)
        {
            InitializeComponent();
            _locSelPage = locSelPage;
        }

        private void Terminate_Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("WARNING!!", "Are you sure you want terminate? All data will be lost", "Yes");
        }

        private async void AdminLogin_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminLoginPage(this));
        }

        private async void UploadData_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuDataHandlerPage(this));
        }

        private void Back_Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Switch_Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Hej", "hej", "Hej");
        }
    }
}