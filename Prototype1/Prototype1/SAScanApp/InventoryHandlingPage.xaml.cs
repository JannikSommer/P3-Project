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
    public partial class InventoryHandlingPage : ContentPage
    {
        public InventoryHandlingPage()
        {
            InitializeComponent();
        }

        private void Add_Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Test", "Test", "Test");
        }

        private void Remove_Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Test", "Test", "Test");
        }
    }
}