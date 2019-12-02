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

        public MainPage()
        {
            InitializeComponent();
            logo.Source = ImageSource.FromResource("SAScanApp.images.salogo.JPG");


        }

        private void BS_Selected(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ScanPage());
        }

        private void CS_Selected(object sender, EventArgs e)
        {
            DisplayAlert("Hey", "Hey", "Hey");
        }

        private void Admin_Selected(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AdminLoginPage(this));
        }
    }
}
