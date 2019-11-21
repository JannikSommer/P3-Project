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
    public partial class M_StartPage : ContentPage
    {
        private BS_StartPage _bsStartPage { get; set; }
        private BS_LocationSelected _bsLocSelPage { get; set; }
        public M_StartPage(BS_StartPage bsStartPage)
        {
            InitializeComponent();
            _bsStartPage = bsStartPage;
        }

        public M_StartPage(BS_LocationSelected bsLocSelPage)
        {
            InitializeComponent();
            _bsLocSelPage = bsLocSelPage;
        }

        private void Terminate_Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("WARNING!!", "Are you sure you want terminate? All data will be lost", "Yes");
        }

        private async void AdminLogin_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminLogin(this));
        }

        private async void UploadData_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new M_UploadData(this));
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