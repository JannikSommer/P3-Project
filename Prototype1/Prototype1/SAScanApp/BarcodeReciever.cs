using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace SAScanApp
{
   public class BarcodeReciever
    {
        string barc = null;
        public ObservableCollection<string> barcodes { get; set; } = new ObservableCollection<string>();
        public string RecieveBarcode(object sender, EventArgs e)
        {

            DependencyService.Get<IBluetoothHandler>().getBarcode();

            var hej = DependencyService.Get<IBluetoothHandler>();

            MessagingCenter.Subscribe<Object, string> (hej, "barcode", (a, s) =>
            {

                barc = a.ToString();
                barcodes.Add(a.ToString());
                
            });

            var Checker = new CheckSum();

            if (Checker.CheckSumValidation(barc))
            {
                
                return barc;
            }

            return null;
        }
    } 
}
    