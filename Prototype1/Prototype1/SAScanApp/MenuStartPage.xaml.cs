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
        public string UserName { get; set; }
        public MenuStartPage(ScanPage scanPage)
        {
            InitializeComponent();
            _scanPage = scanPage;
            UserName = scanPage.UserName;
        }

        public MenuStartPage(LocationSelected locSelPage)
        {
            InitializeComponent();
            _locSelPage = locSelPage;
        }

       

        private async void AdminLogin_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminLoginPage(this));
        }

        private async void GetData_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuDataHandlerPage(this));
        }

        private void Terminate_Button_Clicked(object sender, EventArgs e)
        {
            // Hvad skal vi returnere til serveren? En partition med værdien 'terminated' sat til true/false ?

        }


    }
}