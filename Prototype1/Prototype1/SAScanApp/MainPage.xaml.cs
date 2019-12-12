﻿using System;
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

            Partition = new Partition(new Model.Location("000A01",
                                                                        new List<Item> {
                                                                            new Item("5701872203005"),
                                                                            new Item("64747"),
                                                                            new Item ("8979878"),
                                                                            new Item ("78789"),
                                                                            new Item ("878979")
                                                                            }));

            await Navigation.PushAsync(new ScanPage(Partition));
        }
    }
}
