using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Model;

namespace SAScanApp
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {        //Skal have en constructor der læser partition ind så den kan bruges i ScanPage instansiering i ScanPage_Selected
        public MainPage() { 
            InitializeComponent();
            logo.Source = ImageSource.FromResource("SAScanApp.images.salogo.JPG");
        }

        private async void ScanPage_Selected(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuDataHandlerPage(this));
        }

        private async void Admin_Selected(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminLoginPage(this));
        }

    }
}
