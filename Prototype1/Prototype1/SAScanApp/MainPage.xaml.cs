using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SAScanApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
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
            Navigation.PushAsync(new BS_StartPage());
        }

        private void CS_Selected(object sender, EventArgs e)
        {
            DisplayAlert("Hey", "Hey", "Hey");
        }

        private void Admin_Selected(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AdminLogin(this));
        }
    }
}
