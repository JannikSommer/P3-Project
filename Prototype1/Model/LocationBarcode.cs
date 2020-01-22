using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    
/// <summary> 
/// This class contains every itembarcode scanned at a given location. 
/// Scanning a new location barcode should create a new list such that each location is seperated. 
/// Each item barcode is then enumerated at the server, to increase the quantity of an item. 
/// </summary> 

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