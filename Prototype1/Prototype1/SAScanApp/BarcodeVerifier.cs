using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace SAScanApp {
   public class BarcodeVerifier
    {

        private string barc { get; set; }
        public ObservableCollection<string> barcodes { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// Returns the item corresponding to the scanned barcode. If the item does not exist in the current context <b>null</b> is returned.
        /// </summary>
        /// <param name="partition"></param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public Item GetScannedItem(List<Model.Location> locations, string barcode) {
            foreach(var location in locations) {
                foreach(var item in location.Items) {
                    if(item.ID == barcode)
                        return item;
                }
            }
            return null;
        }

        public bool VerifyBarcode(Partition partition, string barcode) {
            if(GetScannedItem(partition.Locations, barcode) != null)
                return true;
            return false;
        }
    }
}
    