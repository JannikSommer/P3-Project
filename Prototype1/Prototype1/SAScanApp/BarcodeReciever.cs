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
        private string barc { get; set; }
        public ObservableCollection<string> barcodes { get; set; } = new ObservableCollection<string>();
        
        public string RecieveBarcode(Partition partition)
        {
            barc = "xxxxxxxxxx";
            

            MessagingCenter.Subscribe<Object, string> (this, "Barcode", (a, s) =>
            {

                barc = a.ToString();
                barcodes.Add(a.ToString());                           
                
            });

            var Checker = new CheckSum();

            if (Checker.CheckSumValidation(barc) == true)
            {

                List<Item> currentItems = new List<Item>();
                int j = 0;

                for (int i = 0; i < partition.Locations.Count; i++)
                {
                    if (partition.Locations[i].Items[j].ID == barc)
                    {
                        partition.Locations[i].Items[j].CountedQuantity++;

                        return barc;
                    }

                    
                }
            }

            return null;
        }
    }
}
    