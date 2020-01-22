using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ItemBarcode
    {
        public string Barcode { get; set; }
        public int Quantity { get; set; }

        public ItemBarcode(string barcode)
        {
            Barcode = barcode;
        }
    }
}
