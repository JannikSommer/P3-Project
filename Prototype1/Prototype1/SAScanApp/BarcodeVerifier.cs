using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace SAScanApp
{
   public class BarcodeVerifier
    {
        private string barc { get; set; }
        public ObservableCollection<string> barcodes { get; set; } = new ObservableCollection<string>();

        public bool VerifyBarcode(Partition partition, string barcode)
        {
            barc = barcode;

            if (barc.Length == 13)
            {
                for (int i = 0; i < partition.Locations.Count; i++)
                {
                    for (int j = 0; j < partition.Locations[i].Items.Count; j++)
                    {
                        if (partition.Locations[i].Items[j].ID == barc)
                        {
                            //partition.Locations[i].Items[j].CountedQuantity++;
                            return true;
                        }
                    }


                }
            }
           /*MessagingCenter.Subscribe<Object, string> (this, "Barcode", (a, s) => {
                Barcode = a.ToString();
                barcodes.Add(a.ToString());
            }); */
            
            return false;
        }
    }
}
    