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
    {

        public Partition Partition { get; set; }
        //Skal have en constructor der læser partition ind så den kan bruges i ScanPage instansiering i ScanPage_Selected
        public MainPage() {
            InitializeComponent();
            logo.Source = ImageSource.FromResource("SAScanApp.images.salogo.JPG");
        }

        private void ScanPage_Selected(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MenuDataHandlerPage(this));
        }

        private void Admin_Selected(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AdminLoginPage(this));
        }

        private async void Test_Button_Clicked(object sender, EventArgs e)
        {

            Partition = new Partition(new Location("000A01",
                                                                        new List<Item> {
                                                                            new Item("192824442774"),
                                                                            new Item("2999008485538"),
                                                                            new Item ("8979878"),
                                                                            new Item ("5709216007104"),
                                                                            new Item ("54491892")
                                                                            }));

            await Navigation.PushAsync(new ScanPage(Partition));
        }
    }
}
