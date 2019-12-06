using Model;
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
        public string RecieveBarcode(Partition partition)
        {

            var hej = DependencyService.Get<IBluetoothHandler>();

            MessagingCenter.Subscribe<Object, string> (hej, "barcode", (a, s) =>
            {

                barc = a.ToString();
                barcodes.Add(a.ToString());                           
                
            });

            var Checker = new CheckSum();

            if (Checker.CheckSumValidation(barc))
            {

                List<Item> currentItems = new List<Item>();
                int j = 0;

                for (int i = 0; i < partition.Locations.Count; i++)
                {
                    if (partition.Locations[i].Items[j].ID == barcode)
                    {
                        partition.Locations[i].Items[j].CountedQuantity++; ;
            }

            return null;
        }
    }
}
    