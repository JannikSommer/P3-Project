using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SAScanApp
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        //Skal have en constructor der læser partition ind så den kan bruges i ScanPage instansiering i ScanPage_Selected
        public MainPage() {
            InitializeComponent();
            logo.Source = ImageSource.FromResource("SAScanApp.images.salogo.JPG");
        }

        private void ScanPage_Selected(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ScanPage());
        }

        private void Admin_Selected(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AdminLoginPage(this));
        }
    }
}
