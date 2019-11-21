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
    public partial class AdminStartPage : ContentPage
    {
        public AdminStartPage()
        {
            InitializeComponent();
        }

        private void BS_Selected(object sender, EventArgs e)
        {
            DisplayAlert("Hej", "Hej", "Hej");
        }

        private void CS_Selected(object sender, EventArgs e)
        {
            DisplayAlert("Hej", "Hej", "Hej");
        }

        private void Logout_Selected(object sender, EventArgs e)
        {
            DisplayAlert("Hej", "Hej", "Hej");
        }
    }
}