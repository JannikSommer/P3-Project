using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Model
{
    public class ItemBarcode : INotifyPropertyChanged {
        public string Barcode { get; set; }
        public int Quantity { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public ItemBarcode() { }

        public ItemBarcode(string barcode) : this(barcode, 0) {}
        public ItemBarcode(string barcode, int quantity) {
            Barcode = barcode;
            Quantity = quantity;
        }

        protected void OnPropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
