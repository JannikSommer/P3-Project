﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace SAScanApp
{
    public class BarcodeReciever
    {

        public ObservableCollection<string> barcodes { get; set; } = new ObservableCollection<string>();
        public void RecieveBarcode(object sender, EventArgs e)
        {
            /*MessagingCenter.Subscribe<DependencyService.Get<, string>(this, "barcode", (a, s) =>
            {

            }) */
        }
    }
}
