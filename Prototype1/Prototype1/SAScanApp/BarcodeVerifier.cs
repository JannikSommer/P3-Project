using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace SAScanApp {
   public class BarcodeVerifier
    {
        public ObservableCollection<string> barcodes { get; set; } = new ObservableCollection<string>();


        public bool IsLocation(string barcode) {
            if(barcode.Length == 6) {
                if(char.IsDigit(barcode[0]) && char.IsDigit(barcode[1]) && char.IsDigit(barcode[2]) && char.IsLetter(barcode[3]) && char.IsDigit(barcode[4]) && char.IsDigit(barcode[5])) {
                    return true;
                }
            }
            return false;
        }
        

        public ItemBarcode GetScannedItemBarcode(ObservableCollection<ItemBarcode> items, string barcode) {
            foreach(var item in items) {
                if(item.Barcode == barcode) {
                    return item;
                }
            }
            return null;
        }

        public LocationBarcode GetScannedLocationBarcode(ObservableCollection<LocationBarcode> locations, string barcode) {
            foreach(var location in locations) {
                if(location.Barcode == barcode) {
                    return location;
                }
            }
            return null;
        }


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

        public bool VerifyBarcodeObsolete(Partition partition, string barcode) {
            if(GetScannedItem(partition.Locations, barcode) != null)
                return true;
            return false;
        }
    }
}
    