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
        private string Barcode { get; set; }
        public ObservableCollection<string> barcodes { get; set; } = new ObservableCollection<string>();
        
        public string RecieveBarcode(Partition partition)
        {
            Barcode = "xxxxxxxxxx";
            CheckSum Checker = new CheckSum();
            MessagingCenter.Subscribe<Object, string> (this, "Barcode", (a, s) => {
                Barcode = a.ToString();
                barcodes.Add(a.ToString());
            });


            if (Checker.CheckSumValidation(Barcode) == true) {
                List<Item> currentItems = new List<Item>();
                int j = 0;

                for (int i = 0; i < partition.Locations.Count; i++)
                {
                    if (partition.Locations[i].Items[j].ID == Barcode)
                    {
                        partition.Locations[i].Items[j].CountedQuantity++;

                        return Barcode;
                    }

                    
                }
            }

            return null;
        }
    }
}
    