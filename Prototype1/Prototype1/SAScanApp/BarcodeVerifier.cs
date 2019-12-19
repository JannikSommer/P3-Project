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

        /// <summary>
        /// Returns the item corresponding to the scanned barcode. If the item does not exist in the current context <b>null</b> is returned.
        /// </summary>
        /// <param name="partition"></param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public Item GetScannedItem(Partition partition, string barcode) {
            foreach(var location in partition.Locations) {
                foreach(var item in location.Items) {
                    if(item.ID == barcode)
                        return item;
                }
            }
            return null;
        }

        public bool VerifyBarcode(Partition partition, string barcode) {
            if(GetScannedItem(partition, barcode) != null)
                return true;
            return false;
        }
    }
}
    