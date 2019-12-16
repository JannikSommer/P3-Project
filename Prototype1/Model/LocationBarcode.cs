using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class LocationBarcode
    {
        public string Barcode { get; set; }
        public List<ItemBarcode> ItemBarcodes { get; set; }
        
        public LocationBarcode(string barcode)
        {
            Barcode = barcode;
            ItemBarcodes = new List<ItemBarcode>();
        }

        public void AddItemBarcode(string barcode)
        {
            ItemBarcodes.Add(new ItemBarcode(barcode));
        }
    }
}